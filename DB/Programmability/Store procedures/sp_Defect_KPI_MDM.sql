CREATE TYPE IntList AS TABLE
(
	Id int NULL
)

GO

CREATE PROCEDURE [dbo].[sp_Defect_KPI_MDM] (
@CustomerId INT,
	@ServiceDateFrom DATETIME,
	@ServiceDateTo DATETIME,
	@TemplateId INT NULL,
	@OfficeList IntList READONLY,
	@ServiceTypeList IntList READONLY,
	@InvoiceNo nvarchar(100))

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN  TRY

			DECLARE @tblresult TABLE
			(
			bookingNo int,
			serviceDate nvarchar(100),
			customerName nvarchar(1000),
			supplierName nvarchar(1000),
			factoryName nvarchar(1000),
			serviceType nvarchar(1000),
			inspectionStatus nvarchar(1000),
			productRef nvarchar(1000),
			productDesc nvarchar(1000),
			poNumber nvarchar(max),
			poQty int,
			inspectedQty int,
			totalDefects int,
			totalCritical int,
			totalMajor int,
			totalMinor int,
			totalQtyReworked int,
			totalQtyReplaced int,
			totalQtyRejected int,
			finalResult nvarchar(1000),
			productid int,
			reportid int
			)
			DECLARE @tbldefects Table
			(
			bookingno int,
			productid int,
			reportid int,
			totalCritical int,
			totalMajor int,
			totalMinor int,
			totalQtyReworked int,
			totalQtyReplaced int,
			totalQtyRejected int
			)

			insert into @tblresult(bookingNo,serviceDate,customerName,supplierName,factoryName,serviceType,inspectionStatus,productRef,productDesc,
			poQty,poNumber,inspectedQty,finalResult,productid,reportid)
			SELECT insp.Id 'Ins#',case WHEN insp.ServiceDate_From=insp.ServiceDate_To THEN CONVERT(VARCHAR(100), insp.ServiceDate_From,103) else 
			CONVERT(VARCHAR(100), insp.ServiceDate_From,103)+'-'+CONVERT(VARCHAR(100), insp.ServiceDate_To,103) END 'ins date',cu.Customer_Name 'Customer Name',su.Supplier_Name 'Supplier Name'
			,suf.Supplier_Name 'Factory Name' , refs.Name 'Service Type',insps.Status 'Inspection Status',cup.ProductID 'Product Code',
			cup.[Product Description] 'Product Desc', ipt.TotalBookingQuantity 'Po Qty',
			(SELECT STUFF((SELECT   ',' + convert(varchar(1000),cupo.PONO)  from INSP_PurchaseOrder_Transaction ito 
			join CU_PurchaseOrder cupo on cupo.Id=ito.PO_Id
			where cupo.Active=1 AND ito.Inspection_Id=insp.Id and ito.Product_Ref_Id=ipt.Id FOR XML PATH ('')),1,1,''))'PO Number',
			(SELECT sum(ISNULL(fbq.InspectedQuantity,0)) from FB_Report_Quantity_Details fbq 
			where fbq.FbReportDetailId=ipt.Fb_Report_Id and fbq.InspPoTransactionId in (SELECT ipt2.Id from INSP_PurchaseOrder_Transaction ipt2
			where ipt2.Active=1 and ipt2.Inspection_Id=ipt.Inspection_Id and ipt2.Product_Ref_Id=ipt.Id) and fbq.Active=1)'Inspected Qty' ,
			isnull(fb.OverAllResult,'')'Final Result',ipt.Product_Id,ipt.Fb_Report_Id
			from INSP_Transaction insp
			join INSP_Product_Transaction ipt on insp.Id=ipt.Inspection_Id
			left join FB_Report_Details fb on fb.Id=ipt.Fb_Report_Id
			join CU_Products cup on cup.Id=ipt.Product_Id and cup.Active=1
			join CU_Customer cu on cu.Id=insp.Customer_Id
			join SU_Supplier su on su.Id=insp.Supplier_Id
			join SU_Supplier suf on suf.Id=insp.Factory_Id
			join INSP_TRAN_ServiceType ser on ser.Inspection_Id=insp.Id and ser.Active=1
			join REF_ServiceType refs on refs.Id=ser.ServiceType_Id 
			join INSP_Status insps on insps.Id=insp.Status_Id

			where insp.Customer_Id = COALESCE(NULLIF(@CustomerId, ''), insp.Customer_Id) and insp.Status_Id!=4 and ipt.Active=1 and
			insp.ServiceDate_To BETWEEN  @ServiceDateFrom and @ServiceDateTo --mm/dd/yyyy 
			AND (NOT EXISTS(SELECT 1 FROM @OfficeList) OR insp.Office_Id IN (SELECT * FROM @OfficeList))
			AND (NOT EXISTS(SELECT 1 FROM @ServiceTypeList) OR refs.Id IN (SELECT * FROM @ServiceTypeList))
			order by Ins#

			--SELECT * from @tblresult

			INSERT INTO @tbldefects(productid,reportid,totalCritical,totalMajor,totalMinor,totalQtyReworked,totalQtyReplaced,totalQtyRejected,bookingno)
			SELECT ipt.Product_Id,ipt.Fb_Report_Id,sum(isnull( fbq.Critical,0))'critical',
			sum(isnull( fbq.Major,0))'Major',sum(isnull( fbq.Minor,0))'Minor',
			sum(isnull( fbq.Qty_Reworked,0))'Qc Reworked',sum(isnull( fbq.Qty_Replaced,0))'QC Replaced',
			sum(isnull( fbq.Qty_Rejected,0))'QC Rejected' , ipt.Inspection_Id
			from FB_Report_InspDefects fbq
			join FB_Report_Details fbr on fbr.Id=fbq.FbReportDetailId
			join INSP_PurchaseOrder_Transaction  inpo on inpo.Id=fbq.InspPoTransactionId
			join INSP_Product_Transaction ipt on ipt.Id=inpo.Product_Ref_Id
			where ipt.Inspection_Id in (SELECT bookingNo from @tblresult) and fbq.Active=1 and inpo.Active=1 and ipt.Active=1 and fbr.Active=1
			group by ipt.Product_Id,ipt.Fb_Report_Id,ipt.Inspection_Id

			--SELECT * from @tbldefects

			UPDATE r SET  r.totalDefects=(d.totalCritical+d.totalMajor+d.totalMinor),
			r.totalCritical=d.totalCritical,r.totalMajor=d.totalMajor,r.totalMinor=d.totalMinor,
			r.totalQtyReworked=d.totalQtyReworked,r.totalQtyReplaced=d.totalQtyReplaced,r.totalQtyRejected=d.totalQtyRejected
			from @tblresult r join @tbldefects d on r.productid=d.productid and r.reportid=d.reportid and r.bookingNo=d.bookingno 


			SELECT * from @tblresult order by bookingNo


		END TRY

		BEGIN CATCH
			
			THROW

		END CATCH


END
