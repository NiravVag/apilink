using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Inspection;
using DTO.InspectionPicking;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class InspectionPickingManager : IInspectionPickingManager
    {
        private readonly IInspectionPickingRepository _repo = null;
        private readonly IInspectionBookingRepository _repoInspection = null;
        private readonly ICustomerManager _customerManager = null;
        private readonly ICustomerContactManager _customerContactManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly InspectionPickingMap _inspectionpickmap = null;
        public InspectionPickingManager(IInspectionPickingRepository repo, IInspectionBookingRepository repoInspection, IAPIUserContext applicationContext,
                                                                           ICustomerManager customerManager, ICustomerContactManager customerContactManager)
        {
            _repo = repo;
            _repoInspection = repoInspection;
            _customerManager = customerManager;
            _customerContactManager = customerContactManager;
            _ApplicationContext = applicationContext;
            _inspectionpickmap = new InspectionPickingMap();
        }

        /// <summary>
        /// Get Inspection Picking list by booking id
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public async Task<InspectionPickingSummaryResponse> GetPickingDetails(int inspectionId)
        {
            var pickingData = await _repo.GetInspectionPickingByBookingId(inspectionId);

            var response = new InspectionPickingSummaryResponse();

            if (pickingData == null)
            {
                return new InspectionPickingSummaryResponse() { Result = InspectionPickingSummaryResponseResult.datanotfound };
            }

            response.PickingProductList = _inspectionpickmap.GetPickingProductList(pickingData);



            response.InspectionPickingList = _inspectionpickmap.GetInspectionPickingList(pickingData);

            if (response.PickingProductList.Count() == 0)
            {
                response.Result = InspectionPickingSummaryResponseResult.pickingproductnotfound;
            }
            else if (response.PickingProductList != null && response.PickingProductList.Count() > 0 &&
                                            response.InspectionPickingList != null && response.InspectionPickingList.Count() > 0)
            {
                response.Result = InspectionPickingSummaryResponseResult.inspectionpickingproductsfound;
            }
            else if (response.PickingProductList != null && response.PickingProductList.Count() > 0 && response.InspectionPickingList.Count() == 0)
            {
                response.Result = InspectionPickingSummaryResponseResult.pickingproductfound;
            }



            //response.Result = InspectionPickingSummaryResponseResult.success;

            return response;
        }

        /// <summary>
        /// Save Picking Details
        /// </summary>
        /// <param name="inspectionPickingList"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public async Task<SaveInspectionPickingResponse> SavePickingDetails(List<InspectionPickingData> inspectionPickingList, int inspectionId)
        {
            try
            {
                var entity = await _repoInspection.GetInspectionPickingByBookingID(inspectionId);

                if (entity == null)
                    return new SaveInspectionPickingResponse { Result = SaveInspectionPickingResult.InspectionPickingIsNotFound };

                if (inspectionPickingList == null)
                    return new SaveInspectionPickingResponse { Result = SaveInspectionPickingResult.InspectionPickingIsNotFound };

                this.UpdateInspectionPickings(inspectionPickingList, entity);

                await _repoInspection.EditInspectionBooking(entity);

                return new SaveInspectionPickingResponse { Id = entity.Id, Result = SaveInspectionPickingResult.Success };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To update inspection Picking List
        /// </summary>
        /// <param name="inspectionPickingList"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionPickings(List<InspectionPickingData> inspectionPickingList, InspTransaction entity)
        {

            // Existing Picking List if we are not sending in request then remove from the DB
            RemoveInspectionPickingList(inspectionPickingList, entity);

            // Add Picking List From the Request List 
            AddInspectionPickingList(inspectionPickingList, entity);

            // Update Picking List from the Request
            UpdateInspectionPickingList(inspectionPickingList, entity);

        }

        /// <summary>
        /// Add inspection picking list.
        /// </summary>
        /// <param name="pickingList"></param>
        /// <param name="entity"></param>
        private void AddInspectionPickingList(IEnumerable<InspectionPickingData> pickingList, InspTransaction entity)
        {
            // Add inspection picking list
            if (pickingList != null)
            {
                foreach (var item in pickingList.Where(x => x.Id == 0))
                {
                    var inspectionPicking = new InspTranPicking()
                    {
                        CustomerId = item.LabType == (int)LabTypeEnum.Customer ? item.CustomerId : null,
                        LabAddressId = item.LabType == (int)LabTypeEnum.Lab ? item.LabAddressId : null,
                        CusAddressId = item.LabType == (int)LabTypeEnum.Customer ? item.LabAddressId : null,
                        PickingQty = item.PickingQuantity,
                        LabId = item.LabType == (int)LabTypeEnum.Lab ? item.LabId : null,
                        Remarks = item.Remarks,
                        CreatedBy = _ApplicationContext.UserId,
                        CreationDate = DateTime.Now,
                        Active = true


                    };
                    AddORRemoveInspectionPickingContactsList(item.InspectionPickingContacts, inspectionPicking);
                    entity.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && x.Id == item.PoTranId).
                        FirstOrDefault().InspTranPickings.Add(inspectionPicking);
                    entity.InspTranPickings.Add(inspectionPicking);
                    //_repo.AddEntity(inspectionPicking);
                }
            }
        }

        /// <summary>
        /// Add inspection picking list.
        /// </summary>
        /// <param name="pickingList"></param>
        /// <param name="entity"></param>
        private void UpdateInspectionPickingList(IEnumerable<InspectionPickingData> pickingList, InspTransaction entity)
        {
            // Update if data already exist in the db
            var lstInspectionPickingToEdit = new List<InspTranPicking>();

            foreach (var item in pickingList.Where(x => x.Id > 0))
            {
                var inspectionPicking = entity.InspPurchaseOrderTransactions.Where(x => x.Active.HasValue && x.Active.Value && x.Id == item.PoTranId).
                                                SelectMany(x => x.InspTranPickings).Where(x => x.Id == item.Id).FirstOrDefault();

                inspectionPicking.LabId = item.LabType == (int)LabTypeEnum.Lab ? item.LabId : null;
                inspectionPicking.LabAddressId = item.LabType == (int)LabTypeEnum.Lab ? item.LabAddressId : null;
                inspectionPicking.CustomerId = item.LabType == (int)LabTypeEnum.Customer ? item.CustomerId : null;
                inspectionPicking.CusAddressId = item.LabType == (int)LabTypeEnum.Customer ? item.LabAddressId : null;
                inspectionPicking.PickingQty = item.PickingQuantity;
                inspectionPicking.Remarks = item.Remarks;
                inspectionPicking.UpdatedBy = _ApplicationContext.UserId;
                inspectionPicking.UpdationDate = DateTime.Now;
                inspectionPicking.Active = true;
                AddORRemoveInspectionPickingContactsList(item.InspectionPickingContacts, inspectionPicking);
                lstInspectionPickingToEdit.Add(inspectionPicking);
            }

            if (lstInspectionPickingToEdit.Count > 0)
                _repo.EditEntities(lstInspectionPickingToEdit);
        }
        /// <summary>
        /// Remove inspection Picking List if we are not sending in request
        /// </summary>
        /// <param name="pickingList"></param>
        /// <param name="entity"></param>
        private void RemoveInspectionPickingList(IEnumerable<InspectionPickingData> pickingList, InspTransaction entity)
        {
            var pickingListIds = pickingList.Where(x => x.Id > 0).Select(x => x.Id).ToArray();

            var pickingTransactionIds = pickingList.Where(x => x.Id > 0).Select(x => x.PoTranId).ToArray();

            var inspectionPickingListToRemove = new List<InspTranPicking>();

            var inspectionPickings = entity.InspPurchaseOrderTransactions.Where(t => t.Active.HasValue && t.Active.Value)
                                    .SelectMany(x => x.InspTranPickings).Where(x => !pickingListIds.Contains(x.Id));

            foreach (var item in inspectionPickings)
            {
                item.Active = false;
                inspectionPickingListToRemove.Add(item);
            }
            _repo.EditEntities(inspectionPickingListToRemove);
        }


        private void RemoveInspectionPickingContactList(IEnumerable<InspectionPickingContact> pickingContactList, InspTranPicking entity)
        {
            var pickingContactListIds = pickingContactList.Where(x => x.Id > 0).Select(x => x.Id).ToArray();

            var inspectionPickingContactListToRemove = new List<InspTranPickingContact>();

            var inspectionPickingContacts = entity.InspTranPickingContacts.Where(x => !pickingContactListIds.Contains(x.Id));

            foreach (var item in inspectionPickingContacts)
            {
                item.Active = false;
                inspectionPickingContactListToRemove.Add(item);
            }
            _repo.EditEntities(inspectionPickingContactListToRemove);
        }

        /// <summary>
        /// Add or remove inspection picking contact list
        /// </summary>
        /// <param name="pickingList"></param>
        /// <param name="entity"></param>
        /// <param name="isEditBooking"></param>
        private void AddORRemoveInspectionPickingContactsList(IEnumerable<InspectionPickingContact> pickingContactList, InspTranPicking entity)
        {
            if (pickingContactList != null)
            {
                var newContacts = pickingContactList.Where(x => x.Id == 0);

                if (entity.InspTranPickingContacts.Count() > 0)
                {
                    RemoveInspectionPickingContactList(pickingContactList, entity);
                }

                if (newContacts.Count() > 0)
                {
                    foreach (var item in newContacts)
                    {
                        var inspectionPickingContacts = new InspTranPickingContact()
                        {
                            CusContactId = item.CusContactId,
                            LabContactId = item.LabContactId,
                            Active = true
                        };
                        entity.InspTranPickingContacts.Add(inspectionPickingContacts);
                        _repo.AddEntity(inspectionPickingContacts);
                    }
                }

            }
        }

        /// <summary>
        /// Get Customer Address and Customer Contact by Customer Id
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        public async Task<CustomerContactsResponse> GetCustomerContacts(int customerID)
        {

            var response = new CustomerContactsResponse();

            var customerAddress = _customerManager.GetCustomerAddress(customerID);

            var customerContact = await _customerContactManager.GetCustomerContacts(customerID);

            if (customerContact == null)
            {
                return new CustomerContactsResponse() { Result = CustomerContactResponseResult.contactnotfound };
            }

            if (customerAddress == null)
            {
                return new CustomerContactsResponse() { Result = CustomerContactResponseResult.addressnotfound };
            }

            response.CustomerAddressList = customerAddress;

            response.CustomerContactList = customerContact;

            response.Result = CustomerContactResponseResult.success;

            return response;
        }



    }
}
