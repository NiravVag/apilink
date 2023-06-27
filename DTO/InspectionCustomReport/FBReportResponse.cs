using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DTO.CustomReport;
namespace DTO.InspectionCustomReport
{
    public class FBReportResponse
    {
        public int? ReportId { get; set; }
        public string ReportPath { get; set; }
        public string ErrorInfo { get; set; }
    }

    public class FBReportInfoResponse
    {
        public int? ReportMapId { get; set; }
        public int? ReportId { get; set; }
        public List<InspectionCustomReportItem> lstOrderMainDetail { get; set; }
        public List<TemplateDetail> lstTemplateDetail { get; set; }
        public DataTable ChartFacAnalysisDefects { get; set; }
        public DataTable ChartMSOutofTolerance { get; set; }

        public DataTable ChartsGarmentGrade { get; set; }

        public List<Products> lstProducts { get; set; }
        public DataTable ChartDefectsCategory { get; set; }
        public DataTable ChartDefectsCategory2 { get; set; }
        public DataTable ChartDefectsReparability { get; set; }
        public DataTable ChartDefectsWithAql { get; set; }
        public DataTable ChartDefectsWithAql2 { get; set; }
        public List<Sections> lstOrderDetail { get; set; }

        public DataSet dsDefect { get; set; }
        public DataSet dsDefectSpec { get; set; }
        public DataSet dsDefectPacking { get; set; }
        public DataSet dsMSChart { get; set; }

        public DataSet dsColorway { get; set; }

        public DataSet dsProductsColor { get; set; }

        #region Fabric
        public DataTable ChartResultQtyinYards { get; set; }
        public DataTable ChartResultQtyinRolls { get; set; }

        public DataSet dsFabricRubbingTest { get; set; }
        public DataSet dsFabricInspectionDetail { get; set; }
        #endregion
        public TemplateConfigResponse TemplateConfig { get; set; }
    }

    public class TemplateConfigResponse
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? ProductCategoryId { get; set; }
        public bool IsStandardTemplate { get; set; }
        public DateTime? ScheduleFromDate { get; set; }
        public DateTime? ScheduleToDate { get; set; }
        public int? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? DepartId { get; set; }
        public int? FileExtensionID { get; set; }
        public int? ReportToolTypeID { get; set; }

    }


    #region FB order detail
    public class TemplateDetail
    {
        public int? id { get; set; }
        public string ISONo { get; set; }
    }

    #region detail
    public class Template
    {
        /// <summary>
        /// 
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
    }

    public class Products
    {
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? productId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string purchaseOrderNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string productReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string productDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string destinationCountry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Sizes> sizes { get; set; }
    }

    public class Result_and_conclusion_inspected_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_approved_sample_provided_by
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_entry_time
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_exit_time
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_qty_inline
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_qty_warehouse_finished
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_cartons_opened
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_final_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_measured_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_measurement_chart_provided_by
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_number_pcs_measured
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_loop_test
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_number_pcs_measurements_out_tolerance
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_observations
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_order_sheet_provided_by
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_production_at_this_address
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_packing_list_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_qty_per_carton
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_ratio
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_sample_per
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_technical_file_provided_by
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_cartons_opened
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_cartons_presented
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_defect_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_defect_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inspected_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inspected_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_presented_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_measured_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_packed_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_packed_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inspected_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_presented_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_presented_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_presented_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_presented_qty_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_100percent_sample_level
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_presented_qty_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_sample_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_amcharts_defects_with_aql_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? allowed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? found { get; set; }
    }

    public class Result_and_conclusion_amcharts_defects_with_aql
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_amcharts_defects_with_aql_value> result_and_conclusion_amcharts_defects_with_aql_value { get; set; }
    }

    public class Result_and_conclusion_amcharts_measurements_spec_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? value { get; set; }
    }

    public class Result_and_conclusion_amcharts_measurements_spec
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_amcharts_measurements_spec_value> result_and_conclusion_amcharts_measurements_spec_value { get; set; }
    }

    public class Files
    {
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string mediaType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string caption { get; set; }
    }

    public class Result_and_conclusion_your_product
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }



    public class Result_and_conclusion_fields
    {
        #region garment
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_cut_qty> result_and_conclusion_cut_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_cut_qty_percent> result_and_conclusion_cut_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_finished_sewing_qty> result_and_conclusion_finished_sewing_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_finished_sewing_qty_percent> result_and_conclusion_finished_sewing_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_inprocess_packing_qty> result_and_conclusion_inprocess_packing_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_inprocess_packing_qty_percent> result_and_conclusion_inprocess_packing_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_inprocess_sewing_qty> result_and_conclusion_inprocess_sewing_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_inprocess_sewing_qty_percent> result_and_conclusion_inprocess_sewing_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_packing_qty result_and_conclusion_total_inprocess_packing_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_packing_qty_details result_and_conclusion_total_inprocess_packing_qty_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_packing_qty_details_percent result_and_conclusion_total_inprocess_packing_qty_details_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_sewing_qty result_and_conclusion_total_inprocess_sewing_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_sewing_qty_details result_and_conclusion_total_inprocess_sewing_qty_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_sewing_qty_details_percent result_and_conclusion_total_inprocess_sewing_qty_details_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_inspected_qty> result_and_conclusion_inspected_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_packed_qty> result_and_conclusion_packed_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_packed_qty_percent> result_and_conclusion_packed_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_approved_sample_provided_by result_and_conclusion_approved_sample_provided_by { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_cartons_opened result_and_conclusion_cartons_opened { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_entry_time result_and_conclusion_entry_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_exit_time result_and_conclusion_exit_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_qty_inline result_and_conclusion_qty_inline { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_qty_warehouse_finished result_and_conclusion_qty_warehouse_finished { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_final_result result_and_conclusion_final_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_measured_qty result_and_conclusion_measured_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_measurement_chart_provided_by result_and_conclusion_measurement_chart_provided_by { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_number_pcs_measured result_and_conclusion_number_pcs_measured { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_loop_test result_and_conclusion_loop_test { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_number_pcs_measurements_out_tolerance result_and_conclusion_number_pcs_measurements_out_tolerance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_observations result_and_conclusion_observations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_action_taken result_and_conclusion_action_taken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_order_sheet_provided_by result_and_conclusion_order_sheet_provided_by { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_production_at_this_address result_and_conclusion_production_at_this_address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_packing_list_comments result_and_conclusion_packing_list_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_qty_per_carton result_and_conclusion_qty_per_carton { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_ratio result_and_conclusion_ratio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_sample_per result_and_conclusion_sample_per { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_technical_file_provided_by result_and_conclusion_technical_file_provided_by { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_cartons_opened result_and_conclusion_total_cartons_opened { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_cartons_presented result_and_conclusion_total_cartons_presented { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_defect_qty result_and_conclusion_defect_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_defect_qty_percent result_and_conclusion_defect_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_cut_qty result_and_conclusion_total_cut_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_cut_qty_details result_and_conclusion_total_cut_qty_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_cut_qty_details_percent result_and_conclusion_total_cut_qty_details_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_finished_sewing_qty result_and_conclusion_total_finished_sewing_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_finished_sewing_qty_details result_and_conclusion_total_finished_sewing_qty_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_finished_sewing_qty_details_percent result_and_conclusion_total_finished_sewing_qty_details_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inspected_qty result_and_conclusion_total_inspected_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inspected_qty_details result_and_conclusion_total_inspected_qty_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_presented_qty> result_and_conclusion_presented_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_measured_qty_percent result_and_conclusion_measured_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_packed_qty result_and_conclusion_total_packed_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_packed_qty_details result_and_conclusion_total_packed_qty_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_packed_qty_details_percent result_and_conclusion_total_packed_qty_details_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inspected_qty_percent result_and_conclusion_total_inspected_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_presented_qty result_and_conclusion_total_presented_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_presented_qty_details result_and_conclusion_total_presented_qty_details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_presented_qty_percent result_and_conclusion_total_presented_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_presented_qty_result result_and_conclusion_presented_qty_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_sample_number result_and_conclusion_sample_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_amcharts_defects_with_aql result_and_conclusion_amcharts_defects_with_aql { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_cut_qty_percent result_and_conclusion_total_cut_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_finished_sewing_qty_percent result_and_conclusion_total_finished_sewing_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_packing_qty_percent result_and_conclusion_total_inprocess_packing_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inprocess_sewing_qty_percent result_and_conclusion_total_inprocess_sewing_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_packed_qty_percent result_and_conclusion_total_packed_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_presented_qty_result result_and_conclusion_total_presented_qty_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_100percent_sample_level result_and_conclusion_100percent_sample_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_amcharts_measurements_spec result_and_conclusion_amcharts_measurements_spec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_your_product result_and_conclusion_your_product { get; set; }

        public Result_and_conclusion_amcharts_garment_grade result_and_conclusion_amcharts_garment_grade { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_first_choice_qty result_and_conclusion_first_choice_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_second_choice_qty result_and_conclusion_second_choice_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_third_choice_qty result_and_conclusion_third_choice_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_first_choice_qty_percent result_and_conclusion_first_choice_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_second_choice_qty_percent result_and_conclusion_second_choice_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_third_choice_qty_percent result_and_conclusion_third_choice_qty_percent { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_colorway_size_range result_and_conclusion_colorway_size_range { get; set; }
        #endregion 

        #region Fabric

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_fabric_inspection_result result_and_conclusion_fabric_inspection_result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_length_unit result_and_conclusion_length_unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_system_tolerance_roll result_and_conclusion_system_tolerance_roll { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_system_tolerance_shipment result_and_conclusion_system_tolerance_shipment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_accepted_qty result_and_conclusion_total_accepted_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_accepted_qty_percent result_and_conclusion_total_accepted_qty_percent { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_produced_qty result_and_conclusion_total_produced_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_produced_qty_percent result_and_conclusion_total_produced_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_rejected_qty result_and_conclusion_total_rejected_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_rejected_qty_percent result_and_conclusion_total_rejected_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_rejected_qty_rolls result_and_conclusion_total_rejected_qty_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_amcharts_fabric_inspection_detail result_and_conclusion_amcharts_fabric_inspection_detail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_amcharts_fabric_inspection_summary result_and_conclusion_amcharts_fabric_inspection_summary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_approved_fabric_provided_by result_and_conclusion_approved_fabric_provided_by { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_aql_level result_and_conclusion_aql_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_aql_level_custom result_and_conclusion_aql_level_custom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_arrival_date result_and_conclusion_arrival_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_color_conformity_comments result_and_conclusion_color_conformity_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_color_conformity_result result_and_conclusion_color_conformity_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_color_fastness_rubbing_comments result_and_conclusion_color_fastness_rubbing_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_color_fastness_rubbing_result result_and_conclusion_color_fastness_rubbing_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_customer_sealed_sample result_and_conclusion_customer_sealed_sample { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_departure_date result_and_conclusion_departure_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_dyeing_defects_comments result_and_conclusion_dyeing_defects_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_dyeing_defects_result result_and_conclusion_dyeing_defects_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_fabric_inspection_comments result_and_conclusion_fabric_inspection_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_fabric_weight_comments result_and_conclusion_fabric_weight_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_fabric_weight_result result_and_conclusion_fabric_weight_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_general_conformity_comments result_and_conclusion_general_conformity_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_general_conformity_result result_and_conclusion_general_conformity_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_inspector_firstname result_and_conclusion_inspector_firstname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_inspector_lastname result_and_conclusion_inspector_lastname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_inspector_name result_and_conclusion_inspector_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_lab_dip_provided_by result_and_conclusion_lab_dip_provided_by { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_nb_lots_presented result_and_conclusion_nb_lots_presented { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_nb_rolls_presented result_and_conclusion_nb_rolls_presented { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_number_samples_sealed result_and_conclusion_number_samples_sealed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_parcel_number result_and_conclusion_parcel_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_report_date result_and_conclusion_report_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_roll_packing_comments result_and_conclusion_roll_packing_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_roll_packing_result result_and_conclusion_roll_packing_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_sgt_sealed_sample result_and_conclusion_sgt_sealed_sample { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_accepted_qty_rolls result_and_conclusion_total_accepted_qty_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inspected_qty_rolls result_and_conclusion_total_inspected_qty_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_presented_qty_rolls result_and_conclusion_total_presented_qty_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_produced_qty_rolls result_and_conclusion_total_produced_qty_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_warp_comments result_and_conclusion_warp_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_warp_result result_and_conclusion_warp_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_weaving_comments result_and_conclusion_weaving_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_weaving_result result_and_conclusion_weaving_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_weft_comments result_and_conclusion_weft_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_weft_result result_and_conclusion_weft_result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_composition result_and_conclusion_composition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_fabric_code result_and_conclusion_fabric_code { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_style_category result_and_conclusion_style_category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_inspection_site result_and_conclusion_inspection_site { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_garment_vendor_factory result_and_conclusion_garment_vendor_factory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_description_and_qty_of_sealed_samples result_and_conclusion_description_and_qty_of_sealed_samples { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_parcel_awb_no result_and_conclusion_parcel_awb_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_external_lab_used result_and_conclusion_external_lab_used { get; set; }

        /// <summary>
        /// add 20230210
        /// </summary>
        public List<Result_and_conclusion_produced_qty> result_and_conclusion_produced_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_color_conformity_in_uv_light_comments result_and_conclusion_color_conformity_in_uv_light_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_color_conformity_in_uv_light_result result_and_conclusion_color_conformity_in_uv_light_result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_odor_smell_comments result_and_conclusion_odor_smell_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_odor_smell_result result_and_conclusion_odor_smell_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_style_reference result_and_conclusion_style_reference { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_produced_qty_details result_and_conclusion_total_produced_qty_details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_rejected_4_pt_system_qty result_and_conclusion_rejected_4_pt_system_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_rejected_4_pt_system_qty_rolls result_and_conclusion_rejected_4_pt_system_qty_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_rejected_other_qty result_and_conclusion_rejected_other_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_rejected_other_qty_rolls result_and_conclusion_rejected_other_qty_rolls { get; set; }
        #endregion

        #region kith 100

        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_presented_percent> result_and_conclusion_presented_percent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_measured_qty2 result_and_conclusion_measured_qty2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_measured_qty2_percent result_and_conclusion_measured_qty2_percent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_measurements_out_tolerance_percent result_and_conclusion_measurements_out_tolerance_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_measurements_out_tolerance_percent2 result_and_conclusion_measurements_out_tolerance_percent2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_number_pcs_measurements_out_tolerance2 result_and_conclusion_number_pcs_measurements_out_tolerance2 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inspected_qty2 result_and_conclusion_total_inspected_qty2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_inspected_qty2_percent result_and_conclusion_total_inspected_qty2_percent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_total_presented_percent_details result_and_conclusion_total_presented_percent_details { get; set; }

        #endregion
    }

    public class Result_and_conclusion
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_fields result_and_conclusion_fields { get; set; }
    }

    public class Workmanship_cap_display
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_defect_critical_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_defect_critical_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_defect_major_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_defect_major_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_defect_minor_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_defect_minor_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_dynamic_lines_defect
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_correction_actions
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_critical
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_immediate_actions
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_immediate_actions_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_major
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_minor
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_defect_root_cause_analysis
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_position
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_reparability
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_zone
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_defect_percent_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Workmanship_dynamic_lines_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect workmanship_dynamic_lines_defect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_correction_actions workmanship_dynamic_lines_defect_correction_actions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_critical workmanship_dynamic_lines_defect_critical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_description workmanship_dynamic_lines_defect_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_immediate_actions workmanship_dynamic_lines_defect_immediate_actions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_immediate_actions_comment workmanship_dynamic_lines_defect_immediate_actions_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_major workmanship_dynamic_lines_defect_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_minor workmanship_dynamic_lines_defect_minor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_defect_root_cause_analysis workmanship_dynamic_lines_defect_root_cause_analysis { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_position workmanship_dynamic_lines_position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_product workmanship_dynamic_lines_product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_reparability workmanship_dynamic_lines_reparability { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_size workmanship_dynamic_lines_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_zone workmanship_dynamic_lines_zone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_photos workmanship_dynamic_lines_photos { get; set; }

        public Workmanship_dynamic_lines_defect_code workmanship_dynamic_lines_defect_code { get; set; }

        public Workmanship_dynamic_lines_defect_status workmanship_dynamic_lines_defect_status { get; set; }

        public Workmanship_dynamic_lines_percent workmanship_dynamic_lines_percent { get; set; }
        public Workmanship_dynamic_lines_garment_grade workmanship_dynamic_lines_garment_grade { get; set; }

        #region EDEN PARK
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_frequency_same_piece workmanship_dynamic_lines_frequency_same_piece { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_garment_size workmanship_dynamic_lines_garment_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines_inspected_final_inspection workmanship_dynamic_lines_inspected_final_inspection { get; set; }

        #endregion
    }

    public class Workmanship_dynamic_lines_defect_code
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    #region EDEN PARK
    public class Workmanship_dynamic_lines_frequency_same_piece
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_garment_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_inspected_final_inspection
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Packing_labeling_results_chinese_label_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Packing_labeling_results_chinese_label
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Sizes
    {
        /// <summary>
        /// 
        /// </summary>
        public string sizename { get; set; }
    }

    public class Result_and_conclusion_colorway_size_range_colorway_fit
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_colorway_size_range_colorway_grandtotal
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_colorway_size_range_colorway_product_size_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Result_and_conclusion_colorway_size_range_colorway_product_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Result_and_conclusion_colorway_size_range_colorway_size_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string size { get; set; }
    }

    public class Result_and_conclusion_colorway_size_range_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_colorway_size_range_colorway_fit result_and_conclusion_colorway_size_range_colorway_fit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion_colorway_size_range_colorway_grandtotal result_and_conclusion_colorway_size_range_colorway_grandtotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_colorway_size_range_colorway_product_size_total> result_and_conclusion_colorway_size_range_colorway_product_size_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_colorway_size_range_colorway_product_total> result_and_conclusion_colorway_size_range_colorway_product_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_colorway_size_range_colorway_size_total> result_and_conclusion_colorway_size_range_colorway_size_total { get; set; }
    }

    public class Result_and_conclusion_colorway_size_range
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_colorway_size_range_lines> result_and_conclusion_colorway_size_range_lines { get; set; }
    }
    #endregion 

    public class Workmanship_dynamic_lines_defect_status
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_dynamic_lines_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }
    public class Workmanship_dynamic_lines_garment_grade
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }
    public class Workmanship_dynamic_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Workmanship_dynamic_lines_lines> workmanship_dynamic_lines_lines { get; set; }
    }

    public class Workmanship_findings_lines_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_findings_lines_highlight
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_findings_lines_is_automatic
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_findings_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_findings_lines_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_findings_lines_status_line
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_findings_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_findings_lines_description workmanship_findings_lines_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_findings_lines_is_automatic workmanship_findings_lines_is_automatic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_findings_lines_highlight workmanship_findings_lines_highlight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_findings_lines_product workmanship_findings_lines_product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_findings_lines_result workmanship_findings_lines_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_findings_lines_status_line workmanship_findings_lines_status_line { get; set; }
    }

    public class Workmanship_findings_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Workmanship_findings_lines_lines> workmanship_findings_lines_lines { get; set; }
    }

    public class Workmanship_init_check_aql
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_overview_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Workmanship_3_minor_1_major
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Workmanship_5_minor_1_major
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_cap_display workmanship_cap_display { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_defect_critical_result workmanship_defect_critical_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_defect_critical_total workmanship_defect_critical_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_defect_major_result workmanship_defect_major_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_defect_major_total workmanship_defect_major_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_defect_minor_result workmanship_defect_minor_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_defect_minor_total workmanship_defect_minor_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_dynamic_lines workmanship_dynamic_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_findings_lines workmanship_findings_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_init_check_aql workmanship_init_check_aql { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_overall_result workmanship_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_overview_comments workmanship_overview_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_overview_result workmanship_overview_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_action_taken workmanship_action_taken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Workmanship_3_minor_1_major workmanship_3_minor_1_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_5_minor_1_major workmanship_5_minor_1_major { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Workmanship_defect_percent_total workmanship_defect_percent_total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Workmanship_automatic_remark workmanship_automatic_remark { get; set; }
    }

    public class Workmanship_cap
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_cap_fields workmanship_cap_fields { get; set; }
    }
    public class Workmanship_cap_fields
    {
    }
    public class Defects_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Defects_result_fields defects_result_fields { get; set; }
    }
    public class Defects_result_fields
    {
    }

    public class Workmanship_additional_datas_comments_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_additional_datas_comments_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Workmanship_additional_datas_comments_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_additional_datas_comments_comment workmanship_additional_datas_comments_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_additional_datas_comments_product workmanship_additional_datas_comments_product { get; set; }
    }

    public class Workmanship_additional_datas_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Workmanship_additional_datas_comments_lines> workmanship_additional_datas_comments_lines { get; set; }
    }

    public class Workmanship_additional_datas_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_additional_datas_comments workmanship_additional_datas_comments { get; set; }
    }

    public class Workmanship_additional_datas
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_additional_datas_fields workmanship_additional_datas_fields { get; set; }
    }

    public class Workmanship_results_analysis_amcharts_defects_by_category_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? value { get; set; }
    }

    public class Workmanship_results_analysis_amcharts_defects_by_category
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Workmanship_results_analysis_amcharts_defects_by_category_value> workmanship_results_analysis_amcharts_defects_by_category_value { get; set; }
    }

    public class Workmanship_results_analysis_amcharts_defects_with_aql_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? found { get; set; }
    }

    public class Workmanship_results_analysis_amcharts_defects_with_aql
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Workmanship_results_analysis_amcharts_defects_with_aql_value> workmanship_results_analysis_amcharts_defects_with_aql_value { get; set; }
    }

    public class Workmanship_results_analysis_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_results_analysis_amcharts_defects_by_category workmanship_results_analysis_amcharts_defects_by_category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_results_analysis_amcharts_defects_by_reparability workmanship_results_analysis_amcharts_defects_by_reparability { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_results_analysis_amcharts_defects_with_aql workmanship_results_analysis_amcharts_defects_with_aql { get; set; }
    }

    public class Workmanship_results_analysis
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_results_analysis_fields workmanship_results_analysis_fields { get; set; }
    }

    public class Workmanship_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_cap workmanship_cap { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Defects_result defects_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_additional_datas workmanship_additional_datas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing studio_asia_workmanship_packing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status studio_asia_workmanship_report_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_results_analysis workmanship_results_analysis { get; set; }
    }

    public class Workmanship
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_fields workmanship_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_minor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_critical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sample_size_critical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sample_size_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sample_size_minor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowed_critical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowed_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowed_minor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Products> products { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_subSections workmanship_subSections { get; set; }
    }

    public class Workmanship2
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_fields workmanship_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_minor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aql_critical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sample_size_critical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sample_size_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sample_size_minor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowed_critical { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowed_major { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allowed_minor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Products> products { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship_subSections workmanship_subSections { get; set; }
    }
    public class Measurement_and_fitting_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Measurement_and_fitting_overview_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Measurement_and_fitting_findings_lines_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Measurement_and_fitting_findings_lines_is_automatic
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Measurement_and_fitting_findings_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }


    public class Measurement_and_fitting_findings_lines_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }


    public class Measurement_and_fitting_findings_lines_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Measurement_and_fitting_findings_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_findings_lines_description measurement_and_fitting_findings_lines_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_findings_lines_is_automatic measurement_and_fitting_findings_lines_is_automatic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_findings_lines_product measurement_and_fitting_findings_lines_product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_findings_lines_result measurement_and_fitting_findings_lines_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_findings_lines_photos measurement_and_fitting_findings_lines_photos { get; set; }
    }

    public class Measurement_and_fitting_findings_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Measurement_and_fitting_findings_lines_lines> measurement_and_fitting_findings_lines_lines { get; set; }
    }

    public class Measurement_and_fitting_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Measurement_and_fitting_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Measurement_and_fitting_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_action_taken measurement_and_fitting_action_taken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_overview_comments measurement_and_fitting_overview_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_findings_lines measurement_and_fitting_findings_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_overall_result measurement_and_fitting_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_overview_result measurement_and_fitting_overview_result { get; set; }
    }

    public class Measurement_and_fitting_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Measurement_and_fitting_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_photos_photos measurement_and_fitting_photos_photos { get; set; }
    }

    public class Measurement_and_fitting_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_photos_fields measurement_and_fitting_photos_fields { get; set; }
    }

    public class Samples
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool is_out_of_tolerance { get; set; }
    }

    public class Points
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string required { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tolerance_minus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tolerance_plus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool tolerance_plus_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool tolerance_minus_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Samples> samples { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? subtotal_pieces_measured { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? subtotal_out_of_tolerance { get; set; }
    }

    public class Tables
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Points> points { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? total_pieces_measured { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? total_out_of_tolerance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> samples_names { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? samples { get; set; }
    }

    public class Measurement
    {
        /// <summary>
        /// 
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Tables> tables { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? grandtotal_pieces_measured { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? grandtotal_out_of_tolerance { get; set; }
    }

    public class Measurement_and_fitting_results_measurement_fitting
    {
        /// <summary>
        /// 
        /// </summary>
        public Measurement measurement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }

        public List<Files> files { get; set; }
    }

    public class Measurement_and_fitting_results_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Measurement_and_fitting_results_measurement_fitting> measurement_and_fitting_results_measurement_fitting { get; set; }
    }

    public class Measurement_and_fitting_results
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_results_fields measurement_and_fitting_results_fields { get; set; }
    }

    public class Measurement_and_fitting_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_photos measurement_and_fitting_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_results measurement_and_fitting_results { get; set; }
    }

    public class Measurement_and_fitting
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_fields measurement_and_fitting_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting_subSections measurement_and_fitting_subSections { get; set; }
    }

    public class Conformity_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_overview_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_findings_lines_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Conformity_findings_lines_is_automatic
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Conformity_findings_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Conformity_findings_lines_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Conformity_findings_lines_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Conformity_findings_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Conformity_findings_lines_description conformity_findings_lines_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_findings_lines_is_automatic conformity_findings_lines_is_automatic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_findings_lines_product conformity_findings_lines_product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_findings_lines_result conformity_findings_lines_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_findings_lines_photos conformity_findings_lines_photos { get; set; }
    }

    public class Conformity_findings_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Conformity_findings_lines_lines> conformity_findings_lines_lines { get; set; }
    }

    public class Conformity_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Conformity_overall_result conformity_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_overview_comments conformity_overview_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_overview_result conformity_overview_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_action_taken conformity_action_taken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_findings_lines conformity_findings_lines { get; set; }
    }

    public class Conformity_results_approved_sample
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_approved_sample_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_bulk_approved_sample_color
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_bulk_approved_sample_color_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_color
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_color_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_embelishment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_embelishment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_finishing
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_finishing_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_garment_to_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_garment_to_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_hand_feel
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_hand_feel_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_metallic_trims
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_metallic_trims_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_others
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_others_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_plastic_trims
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_plastic_trims_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_technical_file
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_technical_file_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_weight
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_weight_for_sweater
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_weight_for_sweater_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_weight_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_within_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_within_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Conformity_results_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_approved_sample conformity_results_approved_sample { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_approved_sample_comment conformity_results_approved_sample_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_bulk_approved_sample_color conformity_results_bulk_approved_sample_color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_bulk_approved_sample_color_comment conformity_results_bulk_approved_sample_color_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_color conformity_results_color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_color_comment conformity_results_color_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_embelishment conformity_results_embelishment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_embelishment_comment conformity_results_embelishment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_finishing conformity_results_finishing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_finishing_comment conformity_results_finishing_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_garment_to_garment conformity_results_garment_to_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_garment_to_garment_comment conformity_results_garment_to_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_hand_feel conformity_results_hand_feel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_hand_feel_comment conformity_results_hand_feel_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_metallic_trims conformity_results_metallic_trims { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_metallic_trims_comment conformity_results_metallic_trims_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_others conformity_results_others { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_others_comment conformity_results_others_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_plastic_trims conformity_results_plastic_trims { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_plastic_trims_comment conformity_results_plastic_trims_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_technical_file conformity_results_technical_file { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_technical_file_comment conformity_results_technical_file_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_weight conformity_results_weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_weight_for_sweater conformity_results_weight_for_sweater { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_weight_for_sweater_comment conformity_results_weight_for_sweater_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_weight_comment conformity_results_weight_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_within_garment conformity_results_within_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_within_garment_comment conformity_results_within_garment_comment { get; set; }
    }

    public class Conformity_results
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results_fields conformity_results_fields { get; set; }
    }

    public class Conformity_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Conformity_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Conformity_photos_photos conformity_photos_photos { get; set; }
    }

    public class Conformity_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_photos_fields conformity_photos_fields { get; set; }
    }

    public class Conformity_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Conformity_results conformity_results { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_photos conformity_photos { get; set; }
    }

    public class Conformity
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_fields conformity_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity_subSections conformity_subSections { get; set; }
    }

    public class Packing_labeling_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_overview_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_findings_lines_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Packing_labeling_findings_lines_is_automatic
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Packing_labeling_findings_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Packing_labeling_findings_lines_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Packing_labeling_findings_lines_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Packing_labeling_findings_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_findings_lines_description packing_labeling_findings_lines_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_findings_lines_is_automatic packing_labeling_findings_lines_is_automatic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_findings_lines_product packing_labeling_findings_lines_product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_findings_lines_result packing_labeling_findings_lines_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_findings_lines_photos packing_labeling_findings_lines_photos { get; set; }
    }

    public class Packing_labeling_findings_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Packing_labeling_findings_lines_lines> packing_labeling_findings_lines_lines { get; set; }
    }

    public class Packing_labeling_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_action_taken packing_labeling_action_taken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_overall_result packing_labeling_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_overview_comments packing_labeling_overview_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_overview_result packing_labeling_overview_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_findings_lines packing_labeling_findings_lines { get; set; }
    }

    public class Packing_labeling_results_anti_theft
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_anti_theft_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_aspect_packed_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_aspect_packed_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_assortment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_assortment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_barcode
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_barcode_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_brand_label
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_brand_label_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_care_instruction
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_care_instruction_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_closing_method
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_closing_method_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_closing_tape
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_closing_tape_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_dimension_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_dimension_size_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_ply_cardboard_ply
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_ply_cardboard_ply_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_weight
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_weight_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_composition
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_composition_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_empty_space_carton
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_empty_space_carton_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_folding
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_folding_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_hangers
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_hangers_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_hangtag
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_hangtag_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_made_in
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_made_in_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_others
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_others_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class packing_labeling_results_others_labeling
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class packing_labeling_results_others_labeling_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class packing_labeling_results_others_packing
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class packing_labeling_results_others_packing_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Packing_labeling_results_packing_list
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_packing_list_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_polybag_dimension
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_polybag_dimension_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_polybag_holes
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_polybag_holes_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_polybag_marking
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_polybag_marking_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_precut_window_carton
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_precut_window_carton_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_price_tag
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_price_tag_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_qr_code
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_qr_code_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_quantities_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_quantities_size_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_rfid_radio_frequency
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_rfid_radio_frequency_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_shipping_mark
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_shipping_mark_ean
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_shipping_mark_ean_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_shipping_mark_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_silicagel_micropak_humidity
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_silicagel_micropak_humidity_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_silk_tissuepaper
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_silk_tissuepaper_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_size_ring
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_size_ring_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_size_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_smell
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_smell_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    #region  cwf  packing labeling 
    public class Packing_labeling_results_care_label
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_care_label_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_stacking
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_carton_stacking_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_ce_ukca_marking
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_ce_ukca_marking_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_no_bulky_carton
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_no_bulky_carton_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Packing_labeling_results_triman_logo
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Packing_labeling_results_triman_logo_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    #endregion 
    public class Packing_labeling_results_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_anti_theft packing_labeling_results_anti_theft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_anti_theft_comment packing_labeling_results_anti_theft_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_aspect_packed_garment packing_labeling_results_aspect_packed_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_aspect_packed_garment_comment packing_labeling_results_aspect_packed_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_assortment packing_labeling_results_assortment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_assortment_comment packing_labeling_results_assortment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_barcode packing_labeling_results_barcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_barcode_comment packing_labeling_results_barcode_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_brand_label packing_labeling_results_brand_label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_brand_label_comment packing_labeling_results_brand_label_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_care_instruction packing_labeling_results_care_instruction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_care_instruction_comment packing_labeling_results_care_instruction_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_closing_method packing_labeling_results_carton_closing_method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_closing_method_comment packing_labeling_results_carton_closing_method_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_closing_tape packing_labeling_results_carton_closing_tape { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_closing_tape_comment packing_labeling_results_carton_closing_tape_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_dimension_size packing_labeling_results_carton_dimension_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_dimension_size_comment packing_labeling_results_carton_dimension_size_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_ply_cardboard_ply packing_labeling_results_carton_ply_cardboard_ply { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_ply_cardboard_ply_comment packing_labeling_results_carton_ply_cardboard_ply_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_weight packing_labeling_results_carton_weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_weight_comment packing_labeling_results_carton_weight_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_composition packing_labeling_results_composition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_composition_comment packing_labeling_results_composition_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_empty_space_carton packing_labeling_results_empty_space_carton { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_empty_space_carton_comment packing_labeling_results_empty_space_carton_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_folding packing_labeling_results_folding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_folding_comment packing_labeling_results_folding_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_hangers packing_labeling_results_hangers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_hangers_comment packing_labeling_results_hangers_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_hangtag packing_labeling_results_hangtag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_hangtag_comment packing_labeling_results_hangtag_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_made_in packing_labeling_results_made_in { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_made_in_comment packing_labeling_results_made_in_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_others packing_labeling_results_others { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_others_comment packing_labeling_results_others_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public packing_labeling_results_others_labeling packing_labeling_results_others_labeling { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public packing_labeling_results_others_labeling_comment packing_labeling_results_others_labeling_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public packing_labeling_results_others_packing packing_labeling_results_others_packing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public packing_labeling_results_others_packing_comment packing_labeling_results_others_packing_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_packing_list packing_labeling_results_packing_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_packing_list_comment packing_labeling_results_packing_list_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_polybag_dimension packing_labeling_results_polybag_dimension { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_polybag_dimension_comment packing_labeling_results_polybag_dimension_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_polybag_holes packing_labeling_results_polybag_holes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_polybag_holes_comment packing_labeling_results_polybag_holes_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_polybag_marking packing_labeling_results_polybag_marking { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_polybag_marking_comment packing_labeling_results_polybag_marking_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_precut_window_carton packing_labeling_results_precut_window_carton { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_precut_window_carton_comment packing_labeling_results_precut_window_carton_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_price_tag packing_labeling_results_price_tag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_price_tag_comment packing_labeling_results_price_tag_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_qr_code packing_labeling_results_qr_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_qr_code_comment packing_labeling_results_qr_code_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_quantities_size packing_labeling_results_quantities_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_quantities_size_comment packing_labeling_results_quantities_size_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_rfid_radio_frequency packing_labeling_results_rfid_radio_frequency { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_rfid_radio_frequency_comment packing_labeling_results_rfid_radio_frequency_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_shipping_mark packing_labeling_results_shipping_mark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_shipping_mark_ean packing_labeling_results_shipping_mark_ean { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_shipping_mark_ean_comment packing_labeling_results_shipping_mark_ean_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_shipping_mark_comment packing_labeling_results_shipping_mark_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_silicagel_micropak_humidity packing_labeling_results_silicagel_micropak_humidity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_silicagel_micropak_humidity_comment packing_labeling_results_silicagel_micropak_humidity_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_silk_tissuepaper packing_labeling_results_silk_tissuepaper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_silk_tissuepaper_comment packing_labeling_results_silk_tissuepaper_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_size packing_labeling_results_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_size_ring packing_labeling_results_size_ring { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_size_ring_comment packing_labeling_results_size_ring_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_size_comment packing_labeling_results_size_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_smell packing_labeling_results_smell { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_smell_comment packing_labeling_results_smell_comment { get; set; }

        #region EDEN PARK
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_chinese_label_comment packing_labeling_results_chinese_label_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_chinese_label packing_labeling_results_chinese_label { get; set; }
        #endregion 

        #region CWF
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_care_label packing_labeling_results_care_label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_care_label_comment packing_labeling_results_care_label_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_stacking packing_labeling_results_carton_stacking { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_carton_stacking_comment packing_labeling_results_carton_stacking_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_ce_ukca_marking packing_labeling_results_ce_ukca_marking { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_ce_ukca_marking_comment packing_labeling_results_ce_ukca_marking_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_no_bulky_carton packing_labeling_results_no_bulky_carton { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_no_bulky_carton_comment packing_labeling_results_no_bulky_carton_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_triman_logo packing_labeling_results_triman_logo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_triman_logo_comment packing_labeling_results_triman_logo_comment { get; set; }
        #endregion
    }

    public class Packing_labeling_results
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results_fields packing_labeling_results_fields { get; set; }
    }

    public class Packing_labeling_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Packing_labeling_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_photos_photos packing_labeling_photos_photos { get; set; }
    }

    public class Packing_labeling_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_photos_fields packing_labeling_photos_fields { get; set; }
    }

    public class Packing_labeling_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_results packing_labeling_results { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_photos packing_labeling_photos { get; set; }
    }

    public class Packing_labeling
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_fields packing_labeling_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling_subSections packing_labeling_subSections { get; set; }
    }

    public class Functionality_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_overview_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_findings_lines_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Functionality_findings_lines_is_automatic
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Functionality_findings_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Functionality_findings_lines_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Functionality_findings_lines_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Functionality_findings_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Functionality_findings_lines_description functionality_findings_lines_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_findings_lines_is_automatic functionality_findings_lines_is_automatic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_findings_lines_product functionality_findings_lines_product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_findings_lines_result functionality_findings_lines_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_findings_lines_photos functionality_findings_lines_photos { get; set; }
    }

    public class Functionality_findings_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Functionality_findings_lines_lines> functionality_findings_lines_lines { get; set; }
    }

    public class Functionality_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Functionality_action_taken functionality_action_taken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_overall_result functionality_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_overview_comments functionality_overview_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_overview_result functionality_overview_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_findings_lines functionality_findings_lines { get; set; }
    }

    public class Functionality_results_buckle
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_buckle_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_button
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_button_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_color
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_color_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_head_opening
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_head_opening_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_heating
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_heating_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_hoodie
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_hoodie_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_hooks
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_hooks_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_other_closures
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_other_closures_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_others
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_others_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_reversibility
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_reversibility_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_velcro
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_velcro_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_zipper
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_zipper_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Functionality_results_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_buckle functionality_results_buckle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_buckle_comment functionality_results_buckle_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_button functionality_results_button { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_button_comment functionality_results_button_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_color functionality_results_color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_color_comment functionality_results_color_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_head_opening functionality_results_head_opening { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_head_opening_comment functionality_results_head_opening_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_heating functionality_results_heating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_heating_comment functionality_results_heating_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_hoodie functionality_results_hoodie { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_hoodie_comment functionality_results_hoodie_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_hooks functionality_results_hooks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_hooks_comment functionality_results_hooks_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_other_closures functionality_results_other_closures { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_other_closures_comment functionality_results_other_closures_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_others functionality_results_others { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_others_comment functionality_results_others_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_reversibility functionality_results_reversibility { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_reversibility_comment functionality_results_reversibility_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_velcro functionality_results_velcro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_velcro_comment functionality_results_velcro_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_zipper functionality_results_zipper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_zipper_comment functionality_results_zipper_comment { get; set; }
    }

    public class Functionality_results
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results_fields functionality_results_fields { get; set; }
    }

    public class Functionality_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Functionality_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Functionality_photos_photos functionality_photos_photos { get; set; }
    }

    public class Functionality_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_photos_fields functionality_photos_fields { get; set; }
    }

    public class Functionality_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Functionality_results functionality_results { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_photos functionality_photos { get; set; }
    }

    public class Functionality
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_fields functionality_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality_subSections functionality_subSections { get; set; }
    }

    public class Children_safety_overview_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_findings_lines_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Children_safety_findings_lines_is_automatic
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Children_safety_findings_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Children_safety_findings_lines_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Children_safety_findings_lines_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string report_product_id { get; set; }
    }

    public class Children_safety_findings_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_findings_lines_description children_safety_findings_lines_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_findings_lines_is_automatic children_safety_findings_lines_is_automatic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_findings_lines_product children_safety_findings_lines_product { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_findings_lines_result children_safety_findings_lines_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_findings_lines_photos children_safety_findings_lines_photos { get; set; }
    }

    public class Children_safety_findings_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Children_safety_findings_lines_lines> children_safety_findings_lines_lines { get; set; }
    }

    public class Children_safety_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_overview_comments children_safety_overview_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_action_taken children_safety_action_taken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_findings_lines children_safety_findings_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_overall_result children_safety_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_overview_result children_safety_overview_result { get; set; }
    }

    public class Children_safety_results_general_checkpoint_zipper_puller_pull_tab_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoint_zipper_puller_pull_tab_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoint_zipper_puller_pull_tab_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoint_zipper_puller_pull_tab_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoint_zipper_puller_pull_tab_textile_non_textile
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoint_zipper_puller_pull_tab_textile_non_textile_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_0_7_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_0_7_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_0_7_years_8_14_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_0_7_years_8_14_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_3
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_3_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_4
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_4_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_3
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_3_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_a_ring_or_a_slider_to_adjust_the_halter_neck__the_strap_including_the_loop_shall_lie_flat_to_the_body_when_worn
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_a_ring_or_a_slider_to_adjust_the_halter_neck__the_strap_including_the_loop_shall_lie_flat_to_the_body_when_worn_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_clips_or_fastening_of_two_cords_are_permitted_provided_there_are_no_free_ends_when_garment_is_worn
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_clips_or_fastening_of_two_cords_are_permitted_provided_there_are_no_free_ends_when_garment_is_worn_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_all_garments__aus__0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_all_garments__aus__0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_both_age_groups_all_garments_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_both_age_groups_all_garments_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_3
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_3_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_3
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_3_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_decorative_cord_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_decorative_cord_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_both_age_groups_drawstring_functional_cord_decorative_cord_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_both_age_groups_drawstring_functional_cord_decorative_cord_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_above_the_elbow_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_above_the_elbow_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_hanging_below_rolled_up_sleeve_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_hanging_below_rolled_up_sleeve_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_hanging_below_rolled_up_sleeve_0
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_hanging_below_rolled_up_sleeve_0_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_below_7_5_cm_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_below_hem_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_components_clothing
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_components_clothing_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_elastic_cord
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_elastic_cord_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_no_knots
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_no_knots_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_positioned
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_positioned_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_cord_length
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_cord_length_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_elastic_cord
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_elastic_cord_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_0_7_years_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_0_7_years_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_elastic_cord
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_elastic_cord_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_free_end_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_free_end_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_free_end_use_clips
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_free_end_use_clips_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_free_end_use_ring
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_free_end_use_ring_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loops
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loops_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_shoulder_straps
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_shoulder_straps_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_8_14_years_free_ends
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_8_14_years_free_ends_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_both_age_mechanism
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_both_age_mechanism_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_shirts_longer_than_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_shirts_longer_than_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_trousers_longer_than_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_trousers_longer_than_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_garments_aus_end_waist
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_garments_aus_end_waist_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_shirts_free_ends
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_shirts_free_ends_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_shirts_protruding
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_shirts_protruding_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_shirts_toggles
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_shirts_toggles_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_trousers_free_ends
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_trousers_free_ends_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_trousers_no_protruding
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_trousers_no_protruding_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_trousers_toggles
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_trousers_toggles_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_shirts_longer_than_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_shirts_longer_than_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_trousers_longer_than_20
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_trousers_longer_than_20_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_0_7_years_intended_tied
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_0_7_years_intended_tied_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_0_7_years_untied_not_below_hem
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_0_7_years_untied_not_below_hem_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_8_14_years_intended_tied
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_8_14_years_intended_tied_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_both_age_intended_tied
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_both_age_intended_tied_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_forbidden_free_end
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_forbidden_free_end_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_below
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_below_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_bows
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_bows_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_garments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_garments_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_including_toggle
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_including_toggle_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_lower_edges
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_lower_edges_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_free_ends
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_free_ends_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_lower
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_lower_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_tab_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_tab_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_width_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_width_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_bows
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_bows_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_fixed_security
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_fixed_security_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_both_age_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_both_age_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_0_7_years_not_hang_lower
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_0_7_years_not_hang_lower_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_both_age_belt_length_3
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_both_age_belt_length_36
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_both_age_belt_length_36_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_both_age_belt_length_3_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_both_age_ornaments_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_both_age_ornaments_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_adjusting_tab_both_age_tab_length_10
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_adjusting_tab_both_age_tab_length_10_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_adjusting_tab_both_age_width_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_adjusting_tab_both_age_width_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_cords_positioned_above_elbow_cord_length
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_cords_positioned_above_elbow_cord_length_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_decorative_cord_not_hang_lower
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_decorative_cord_not_hang_lower_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_functional_cord_not_hang_lower
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_functional_cord_not_hang_lower_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_0_7_years_hanging_below_sleeve_tab_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_0_7_years_hanging_below_sleeve_tab_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_7_14_years_hanging_below_sleeve_tab_length_10
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_7_14_years_hanging_below_sleeve_tab_length_10_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_both_age_above_elbow_width_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_both_age_above_elbow_width_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_tab_length_10
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_tab_length_10_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_width_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_width_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_short_sleeves_0_7_years_drawstring_sleeve_above_elbow_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_short_sleeves_0_7_years_drawstring_sleeve_above_elbow_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_short_sleeves_8_14_years_drawstring_sleeve_above_elbow_cord_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_short_sleeves_8_14_years_drawstring_sleeve_above_elbow_cord_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_short_sleeves_both_age_drawstring_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_short_sleeves_both_age_drawstring_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_components_clothing
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_components_clothing_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_height
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_height_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_no_button
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_no_button_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_tab_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_tab_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_0_7_years_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_0_7_years_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_drawstrings
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_drawstrings_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_elastic_cord
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_elastic_cord_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_functional_cords
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_functional_cords_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_garment_opening
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_garment_opening_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_toggles
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_toggles_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_all_garments_waist_area_max_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_all_garments_waist_area_max_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_below_7_5_cm
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_below_hem_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoint_zipper_puller_pull_tab_0 children_safety_results_general_checkpoint_zipper_puller_pull_tab_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoint_zipper_puller_pull_tab_0_comment children_safety_results_general_checkpoint_zipper_puller_pull_tab_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoint_zipper_puller_pull_tab_1 children_safety_results_general_checkpoint_zipper_puller_pull_tab_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoint_zipper_puller_pull_tab_1_comment children_safety_results_general_checkpoint_zipper_puller_pull_tab_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoint_zipper_puller_pull_tab_textile_non_textile children_safety_results_general_checkpoint_zipper_puller_pull_tab_textile_non_textile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoint_zipper_puller_pull_tab_textile_non_textile_comment children_safety_results_general_checkpoint_zipper_puller_pull_tab_textile_non_textile_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_0_7_years_0 children_safety_results_zone_a_drawstring_0_7_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_0_7_years_0_comment children_safety_results_zone_a_drawstring_0_7_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_0_7_years_8_14_years_0 children_safety_results_zone_a_drawstring_0_7_years_8_14_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_0_7_years_8_14_years_0_comment children_safety_results_zone_a_drawstring_0_7_years_8_14_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_0 children_safety_results_zone_a_drawstring_functional_cord_0_7_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_0_comment children_safety_results_zone_a_drawstring_functional_cord_0_7_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_0 children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_0_comment children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_1 children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_1_comment children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_2 children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_2_comment children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_3 children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_3_comment children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_3_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_4 children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_4_comment children_safety_results_zone_a_drawstring_functional_cord_0_7_years_8_14_years_4_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_1 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_1_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_8_14_years_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_1 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_1_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_2 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_2_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_3 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_3_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_0_7_years_on_a_hood_or_back_of_neck_other_parts_of_zone_a_3_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_1 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_1_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_2 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_2_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_both_age_groups_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_1 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_1_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_2 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_2_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_both_age_groups_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_both_age_groups_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_adjusting_tab_shoulder_strap_0_7_years_8___14_years_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_0 children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_0_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_a_ring_or_a_slider_to_adjust_the_halter_neck__the_strap_including_the_loop_shall_lie_flat_to_the_body_when_worn children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_a_ring_or_a_slider_to_adjust_the_halter_neck__the_strap_including_the_loop_shall_lie_flat_to_the_body_when_worn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_a_ring_or_a_slider_to_adjust_the_halter_neck__the_strap_including_the_loop_shall_lie_flat_to_the_body_when_worn_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_a_ring_or_a_slider_to_adjust_the_halter_neck__the_strap_including_the_loop_shall_lie_flat_to_the_body_when_worn_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_clips_or_fastening_of_two_cords_are_permitted_provided_there_are_no_free_ends_when_garment_is_worn children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_clips_or_fastening_of_two_cords_are_permitted_provided_there_are_no_free_ends_when_garment_is_worn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_clips_or_fastening_of_two_cords_are_permitted_provided_there_are_no_free_ends_when_garment_is_worn_comment children_safety_results_zone_a_drawstring_functional_cord_decorative_cord_halter_neck_neck_cord_bathing_suit_both_age_groups_free_end_the_use_of_clips_or_fastening_of_two_cords_are_permitted_provided_there_are_no_free_ends_when_garment_is_worn_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0 children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_1 children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_1_comment children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_2 children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_2_comment children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0 children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_1 children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_1_comment children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_2 children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_2_comment children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_all_garments__aus__0 children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_all_garments__aus__0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_all_garments__aus__0_comment children_safety_results_zone_b_drawstring_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_all_garments__aus__0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0 children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0 children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment children_safety_results_zone_b_drawstring_functional_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_both_age_groups_all_garments_0 children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_both_age_groups_all_garments_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_both_age_groups_all_garments_0_comment children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_both_age_groups_all_garments_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0 children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0_comment children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_1 children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_1_comment children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_0 children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_0_comment children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_both_age_groups_0 children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_both_age_groups_0_comment children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_8___14_years_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0 children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0 children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment children_safety_results_zone_b_drawstring_functional_cord_decorative_cord_both_age_groups_trousers__shorts__skirts__briefs__bikini_bottoms_shirts__coats__dresses__and_dungarees_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_0 children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_0_comment children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_1 children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_1_comment children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_2 children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_2_comment children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_adjusting_tab_both_age_groups_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_0 children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_0_comment children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_1 children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_1_comment children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_2 children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_2_comment children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_3 children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_3_comment children_safety_results_zone_c_drawstrings_functional_cord_decorative_cord_both_age_groups_3_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_both_age_groups_0 children_safety_results_zone_d_drawstring_functional_cords_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_both_age_groups_0_comment children_safety_results_zone_d_drawstring_functional_cords_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_0 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_0_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_1 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_1_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_2 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_2_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_3 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_3_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_both_age_groups_3_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_0 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_0_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_1 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_1_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_2 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_2_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_adjusting_tab_tied_belt_sashes_bow_ties_0___7_years_both_age_groups_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_0 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_0_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_1 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_1_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_2 children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_2_comment children_safety_results_zone_d_drawstring_functional_cords_decorative_cord_both_age_groups_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_0 children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_0_comment children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_decorative_cord_0 children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_decorative_cord_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_decorative_cord_0_comment children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_both_age_groups_functional_cord_decorative_cord_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0 children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0_comment children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_1 children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_1_comment children_safety_results_zone_e_long_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0 children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0_comment children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_cords_positioned_above_the_elbow_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_both_age_groups_drawstring_functional_cord_decorative_cord_0 children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_both_age_groups_drawstring_functional_cord_decorative_cord_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_both_age_groups_drawstring_functional_cord_decorative_cord_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_both_age_groups_drawstring_functional_cord_decorative_cord_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0 children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_8_14_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0 children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_0_7_years_drawstring_functional_cord_decorative_cord_sleeve_above_the_elbow_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_0 children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_1 children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_1_comment children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_both_age_groups_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_0 children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_1 children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_1_comment children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_above_the_elbow_0 children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_above_the_elbow_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_above_the_elbow_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_both_age_groups_below_the_elbow_above_the_elbow_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_hanging_below_rolled_up_sleeve_0 children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_hanging_below_rolled_up_sleeve_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_hanging_below_rolled_up_sleeve_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_7___14_years_hanging_below_rolled_up_sleeve_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_hanging_below_rolled_up_sleeve_0 children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_hanging_below_rolled_up_sleeve_0 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_hanging_below_rolled_up_sleeve_0_comment children_safety_results_zone_e_long_sleeves_short_sleeves_adjusting_tab_roll_up_sleeve_tab_0___7_years_hanging_below_rolled_up_sleeve_0_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_below_7_5_cm_comment children_safety_results_general_checkpoints_zipper_below_7_5_cm_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_below_hem_forbidden_comment children_safety_results_general_checkpoints_zipper_below_hem_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_components_clothing children_safety_results_general_checkpoints_zipper_components_clothing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_components_clothing_comment children_safety_results_general_checkpoints_zipper_components_clothing_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_forbidden children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_forbidden_comment children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_elastic_cord children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_elastic_cord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_elastic_cord_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_elastic_cord_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_no_knots children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_no_knots { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_no_knots_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_no_knots_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_positioned children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_positioned { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_positioned_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_positioned_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_cord_length children_safety_results_zone_a_decorative_cord_8_14_years_cord_length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_cord_length_comment children_safety_results_zone_a_decorative_cord_8_14_years_cord_length_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_elastic_cord children_safety_results_zone_a_decorative_cord_8_14_years_elastic_cord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_elastic_cord_comment children_safety_results_zone_a_decorative_cord_8_14_years_elastic_cord_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_0_7_years_forbidden children_safety_results_zone_a_drawstring_0_7_years_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_0_7_years_forbidden_comment children_safety_results_zone_a_drawstring_0_7_years_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_elastic_cord children_safety_results_zone_a_drawstring_8_14_years_elastic_cord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_elastic_cord_comment children_safety_results_zone_a_drawstring_8_14_years_elastic_cord_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_free_end_forbidden children_safety_results_zone_a_halter_neck_both_age_free_end_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_free_end_forbidden_comment children_safety_results_zone_a_halter_neck_both_age_free_end_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_free_end_use_clips children_safety_results_zone_a_halter_neck_both_age_free_end_use_clips { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_free_end_use_clips_comment children_safety_results_zone_a_halter_neck_both_age_free_end_use_clips_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_free_end_use_ring children_safety_results_zone_a_halter_neck_both_age_free_end_use_ring { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_free_end_use_ring_comment children_safety_results_zone_a_halter_neck_both_age_free_end_use_ring_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loops children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loops { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loops_comment children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loops_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_comment children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_shoulder_straps children_safety_results_zone_a_shoulder_strap_0_7_years_shoulder_straps { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_shoulder_straps_comment children_safety_results_zone_a_shoulder_strap_0_7_years_shoulder_straps_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_8_14_years_free_ends children_safety_results_zone_a_shoulder_strap_8_14_years_free_ends { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_8_14_years_free_ends_comment children_safety_results_zone_a_shoulder_strap_8_14_years_free_ends_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_both_age_mechanism children_safety_results_zone_a_shoulder_strap_both_age_mechanism { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_both_age_mechanism_comment children_safety_results_zone_a_shoulder_strap_both_age_mechanism_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_shirts_longer_than_14 children_safety_results_zone_b_decorative_cord_both_age_shirts_longer_than_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_shirts_longer_than_14_comment children_safety_results_zone_b_decorative_cord_both_age_shirts_longer_than_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_trousers_longer_than_14 children_safety_results_zone_b_decorative_cord_both_age_trousers_longer_than_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_trousers_longer_than_14_comment children_safety_results_zone_b_decorative_cord_both_age_trousers_longer_than_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_garments_aus_end_waist children_safety_results_zone_b_drawstring_both_age_garments_aus_end_waist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_garments_aus_end_waist_comment children_safety_results_zone_b_drawstring_both_age_garments_aus_end_waist_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_shirts_free_ends children_safety_results_zone_b_drawstring_both_age_shirts_free_ends { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_shirts_free_ends_comment children_safety_results_zone_b_drawstring_both_age_shirts_free_ends_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_shirts_protruding children_safety_results_zone_b_drawstring_both_age_shirts_protruding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_shirts_protruding_comment children_safety_results_zone_b_drawstring_both_age_shirts_protruding_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_shirts_toggles children_safety_results_zone_b_drawstring_both_age_shirts_toggles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_shirts_toggles_comment children_safety_results_zone_b_drawstring_both_age_shirts_toggles_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_trousers_free_ends children_safety_results_zone_b_drawstring_both_age_trousers_free_ends { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_trousers_free_ends_comment children_safety_results_zone_b_drawstring_both_age_trousers_free_ends_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_trousers_no_protruding children_safety_results_zone_b_drawstring_both_age_trousers_no_protruding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_trousers_no_protruding_comment children_safety_results_zone_b_drawstring_both_age_trousers_no_protruding_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_trousers_toggles children_safety_results_zone_b_drawstring_both_age_trousers_toggles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_trousers_toggles_comment children_safety_results_zone_b_drawstring_both_age_trousers_toggles_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_shirts_longer_than_14 children_safety_results_zone_b_functional_cord_both_age_shirts_longer_than_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_shirts_longer_than_14_comment children_safety_results_zone_b_functional_cord_both_age_shirts_longer_than_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_trousers_longer_than_20 children_safety_results_zone_b_functional_cord_both_age_trousers_longer_than_20 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_trousers_longer_than_20_comment children_safety_results_zone_b_functional_cord_both_age_trousers_longer_than_20_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_0_7_years_intended_tied children_safety_results_zone_b_tied_belt_0_7_years_intended_tied { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_0_7_years_intended_tied_comment children_safety_results_zone_b_tied_belt_0_7_years_intended_tied_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_0_7_years_untied_not_below_hem children_safety_results_zone_b_tied_belt_0_7_years_untied_not_below_hem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_0_7_years_untied_not_below_hem_comment children_safety_results_zone_b_tied_belt_0_7_years_untied_not_below_hem_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_8_14_years_intended_tied children_safety_results_zone_b_tied_belt_8_14_years_intended_tied { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_8_14_years_intended_tied_comment children_safety_results_zone_b_tied_belt_8_14_years_intended_tied_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_both_age_intended_tied children_safety_results_zone_b_tied_belt_both_age_intended_tied { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_both_age_intended_tied_comment children_safety_results_zone_b_tied_belt_both_age_intended_tied_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_forbidden_free_end children_safety_results_zone_c_adjusting_tab_both_age_forbidden_free_end { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_forbidden_free_end_comment children_safety_results_zone_c_adjusting_tab_both_age_forbidden_free_end_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_below children_safety_results_zone_c_adjusting_tab_both_age_not_hang_below { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_below_comment children_safety_results_zone_c_adjusting_tab_both_age_not_hang_below_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_14 children_safety_results_zone_c_adjusting_tab_both_age_tab_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_14_comment children_safety_results_zone_c_adjusting_tab_both_age_tab_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_bows children_safety_results_zone_c_drawstrings_both_age_bows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_bows_comment children_safety_results_zone_c_drawstrings_both_age_bows_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_garments children_safety_results_zone_c_drawstrings_both_age_garments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_garments_comment children_safety_results_zone_c_drawstrings_both_age_garments_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_including_toggle children_safety_results_zone_c_drawstrings_both_age_including_toggle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_including_toggle_comment children_safety_results_zone_c_drawstrings_both_age_including_toggle_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_lower_edges children_safety_results_zone_c_drawstrings_both_age_lower_edges { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_lower_edges_comment children_safety_results_zone_c_drawstrings_both_age_lower_edges_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_free_ends children_safety_results_zone_d_adjusting_tab_both_age_free_ends { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_free_ends_comment children_safety_results_zone_d_adjusting_tab_both_age_free_ends_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_lower children_safety_results_zone_d_adjusting_tab_both_age_not_hang_lower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_lower_comment children_safety_results_zone_d_adjusting_tab_both_age_not_hang_lower_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_tab_length_7_5 children_safety_results_zone_d_adjusting_tab_both_age_tab_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_tab_length_7_5_comment children_safety_results_zone_d_adjusting_tab_both_age_tab_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_width_2 children_safety_results_zone_d_adjusting_tab_both_age_width_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_width_2_comment children_safety_results_zone_d_adjusting_tab_both_age_width_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_bows children_safety_results_zone_d_decorative_cord_both_age_bows { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_bows_comment children_safety_results_zone_d_decorative_cord_both_age_bows_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_fixed_security children_safety_results_zone_d_decorative_cord_both_age_fixed_security { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_fixed_security_comment children_safety_results_zone_d_decorative_cord_both_age_fixed_security_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_length_7_5 children_safety_results_zone_d_decorative_cord_both_age_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_length_7_5_comment children_safety_results_zone_d_decorative_cord_both_age_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_both_age_forbidden children_safety_results_zone_d_drawstring_both_age_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_both_age_forbidden_comment children_safety_results_zone_d_drawstring_both_age_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_0_7_years_not_hang_lower children_safety_results_zone_d_tied_belt_0_7_years_not_hang_lower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_0_7_years_not_hang_lower_comment children_safety_results_zone_d_tied_belt_0_7_years_not_hang_lower_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_both_age_belt_length_3 children_safety_results_zone_d_tied_belt_both_age_belt_length_3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_both_age_belt_length_36 children_safety_results_zone_d_tied_belt_both_age_belt_length_36 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_both_age_belt_length_36_comment children_safety_results_zone_d_tied_belt_both_age_belt_length_36_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_both_age_belt_length_3_comment children_safety_results_zone_d_tied_belt_both_age_belt_length_3_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_both_age_ornaments_forbidden children_safety_results_zone_d_tied_belt_both_age_ornaments_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_both_age_ornaments_forbidden_comment children_safety_results_zone_d_tied_belt_both_age_ornaments_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_adjusting_tab_both_age_tab_length_10 children_safety_results_zone_e_adjusting_tab_both_age_tab_length_10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_adjusting_tab_both_age_tab_length_10_comment children_safety_results_zone_e_adjusting_tab_both_age_tab_length_10_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_adjusting_tab_both_age_width_2 children_safety_results_zone_e_adjusting_tab_both_age_width_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_adjusting_tab_both_age_width_2_comment children_safety_results_zone_e_adjusting_tab_both_age_width_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_cords_positioned_above_elbow_cord_length children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_cords_positioned_above_elbow_cord_length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_cords_positioned_above_elbow_cord_length_comment children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_cords_positioned_above_elbow_cord_length_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_14 children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_14_comment children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_7_5 children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_7_5_comment children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_cords_positioned_above_elbow_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_decorative_cord_not_hang_lower children_safety_results_zone_e_long_sleeves_both_age_decorative_cord_not_hang_lower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_decorative_cord_not_hang_lower_comment children_safety_results_zone_e_long_sleeves_both_age_decorative_cord_not_hang_lower_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_functional_cord_not_hang_lower children_safety_results_zone_e_long_sleeves_both_age_functional_cord_not_hang_lower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_functional_cord_not_hang_lower_comment children_safety_results_zone_e_long_sleeves_both_age_functional_cord_not_hang_lower_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_0_7_years_hanging_below_sleeve_tab_length_7_5 children_safety_results_zone_e_roll_up_sleeve_0_7_years_hanging_below_sleeve_tab_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_0_7_years_hanging_below_sleeve_tab_length_7_5_comment children_safety_results_zone_e_roll_up_sleeve_0_7_years_hanging_below_sleeve_tab_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_7_14_years_hanging_below_sleeve_tab_length_10 children_safety_results_zone_e_roll_up_sleeve_7_14_years_hanging_below_sleeve_tab_length_10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_7_14_years_hanging_below_sleeve_tab_length_10_comment children_safety_results_zone_e_roll_up_sleeve_7_14_years_hanging_below_sleeve_tab_length_10_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_both_age_above_elbow_width_2 children_safety_results_zone_e_roll_up_sleeve_both_age_above_elbow_width_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_both_age_above_elbow_width_2_comment children_safety_results_zone_e_roll_up_sleeve_both_age_above_elbow_width_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_tab_length_10 children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_tab_length_10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_tab_length_10_comment children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_tab_length_10_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_width_2 children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_width_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_width_2_comment children_safety_results_zone_e_roll_up_sleeve_both_age_below_elbow_width_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_short_sleeves_0_7_years_drawstring_sleeve_above_elbow_cord_length_7_5 children_safety_results_zone_e_short_sleeves_0_7_years_drawstring_sleeve_above_elbow_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_short_sleeves_0_7_years_drawstring_sleeve_above_elbow_cord_length_7_5_comment children_safety_results_zone_e_short_sleeves_0_7_years_drawstring_sleeve_above_elbow_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_short_sleeves_8_14_years_drawstring_sleeve_above_elbow_cord_length_14 children_safety_results_zone_e_short_sleeves_8_14_years_drawstring_sleeve_above_elbow_cord_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_short_sleeves_8_14_years_drawstring_sleeve_above_elbow_cord_length_14_comment children_safety_results_zone_e_short_sleeves_8_14_years_drawstring_sleeve_above_elbow_cord_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_short_sleeves_both_age_drawstring_bows_forbidden children_safety_results_zone_e_short_sleeves_both_age_drawstring_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_short_sleeves_both_age_drawstring_bows_forbidden_comment children_safety_results_zone_e_short_sleeves_both_age_drawstring_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_photos children_safety_results_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_components_clothing children_safety_results_general_checkpoints_components_clothing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_components_clothing_comment children_safety_results_general_checkpoints_components_clothing_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_height children_safety_results_zone_a_adjusting_tab_both_age_height { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_height_comment children_safety_results_zone_a_adjusting_tab_both_age_height_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_no_button children_safety_results_zone_a_adjusting_tab_both_age_no_button { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_no_button_comment children_safety_results_zone_a_adjusting_tab_both_age_no_button_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_tab_length_7_5 children_safety_results_zone_a_adjusting_tab_both_age_tab_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_tab_length_7_5_comment children_safety_results_zone_a_adjusting_tab_both_age_tab_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_0_7_years_forbidden children_safety_results_zone_a_functional_cord_0_7_years_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_0_7_years_forbidden_comment children_safety_results_zone_a_functional_cord_0_7_years_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_drawstrings children_safety_results_zone_a_functional_cord_8_14_years_drawstrings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_drawstrings_comment children_safety_results_zone_a_functional_cord_8_14_years_drawstrings_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_elastic_cord children_safety_results_zone_a_functional_cord_8_14_years_elastic_cord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_elastic_cord_comment children_safety_results_zone_a_functional_cord_8_14_years_elastic_cord_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_functional_cords children_safety_results_zone_a_functional_cord_8_14_years_functional_cords { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_functional_cords_comment children_safety_results_zone_a_functional_cord_8_14_years_functional_cords_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_garment_opening children_safety_results_zone_a_functional_cord_8_14_years_garment_opening { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_garment_opening_comment children_safety_results_zone_a_functional_cord_8_14_years_garment_opening_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_toggles children_safety_results_zone_a_functional_cord_8_14_years_toggles { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_toggles_comment children_safety_results_zone_a_functional_cord_8_14_years_toggles_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_all_garments_waist_area_max_14 children_safety_results_zone_b_adjusting_tab_both_age_all_garments_waist_area_max_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_all_garments_waist_area_max_14_comment children_safety_results_zone_b_adjusting_tab_both_age_all_garments_waist_area_max_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_below_7_5_cm children_safety_results_general_checkpoints_zipper_below_7_5_cm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_below_hem_forbidden children_safety_results_general_checkpoints_zipper_below_hem_forbidden { get; set; }

        #region update for General safety
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_children_safety_visible children_safety_results_children_safety_visible { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_visible children_safety_results_general_safety_visible { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_button_pulling_test children_safety_results_general_safety_button_pulling_test { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_button_pulling_test_comment children_safety_results_general_safety_button_pulling_test_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_hazardous_pollution children_safety_results_general_safety_hazardous_pollution { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_hazardous_pollution_comment children_safety_results_general_safety_hazardous_pollution_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_needle_book children_safety_results_general_safety_needle_book { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_needle_book_comment children_safety_results_general_safety_needle_book_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_needle_detection children_safety_results_general_safety_needle_detection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_needle_detection_comment children_safety_results_general_safety_needle_detection_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_others children_safety_results_general_safety_others { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_others_comment children_safety_results_general_safety_others_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_small_parts_pulling_test children_safety_results_general_safety_small_parts_pulling_test { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_small_parts_pulling_test_comment children_safety_results_general_safety_small_parts_pulling_test_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_warning_poly_bags children_safety_results_general_safety_warning_poly_bags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_warning_poly_bags_comment children_safety_results_general_safety_warning_poly_bags_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_zipper_pulling_test children_safety_results_general_safety_zipper_pulling_test { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_safety_zipper_pulling_test_comment children_safety_results_general_safety_zipper_pulling_test_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_photos_children_safety children_safety_results_photos_children_safety { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_photos_general_safety children_safety_results_photos_general_safety { get; set; }
        #endregion

        #region update for safety-> General  checkpoints
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_buckle children_safety_results_general_checkpoints_functionality_buckle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_buckle_comment children_safety_results_general_checkpoints_functionality_buckle_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_button children_safety_results_general_checkpoints_functionality_button { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_button_comment children_safety_results_general_checkpoints_functionality_button_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_hooks children_safety_results_general_checkpoints_functionality_hooks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_hooks_comment children_safety_results_general_checkpoints_functionality_hooks_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_other_closures children_safety_results_general_checkpoints_functionality_other_closures { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_other_closures_comment children_safety_results_general_checkpoints_functionality_other_closures_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_velcro children_safety_results_general_checkpoints_functionality_velcro { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_velcro_comment children_safety_results_general_checkpoints_functionality_velcro_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_zipper children_safety_results_general_checkpoints_functionality_zipper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_functionality_zipper_comment children_safety_results_general_checkpoints_functionality_zipper_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_hazardous_pollution children_safety_results_general_checkpoints_general_safety_hazardous_pollution { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_hazardous_pollution_comment children_safety_results_general_checkpoints_general_safety_hazardous_pollution_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_needle_book children_safety_results_general_checkpoints_general_safety_needle_book { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_needle_book_comment children_safety_results_general_checkpoints_general_safety_needle_book_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_needle_detection children_safety_results_general_checkpoints_general_safety_needle_detection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_needle_detection_comment children_safety_results_general_checkpoints_general_safety_needle_detection_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_others children_safety_results_general_checkpoints_general_safety_others { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_others_comment children_safety_results_general_checkpoints_general_safety_others_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_pulling_test children_safety_results_general_checkpoints_general_safety_pulling_test { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_pulling_test_comment children_safety_results_general_checkpoints_general_safety_pulling_test_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_warning_polybag children_safety_results_general_checkpoints_general_safety_warning_polybag { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_warning_polybag_comment children_safety_results_general_checkpoints_general_safety_warning_polybag_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_head_opening children_safety_results_general_checkpoints_other_functions_head_opening { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_head_opening_comment children_safety_results_general_checkpoints_other_functions_head_opening_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_hoodie_garage children_safety_results_general_checkpoints_other_functions_hoodie_garage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_hoodie_garage_comment children_safety_results_general_checkpoints_other_functions_hoodie_garage_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_others children_safety_results_general_checkpoints_other_functions_others { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_others_comment children_safety_results_general_checkpoints_other_functions_others_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_removable_parts children_safety_results_general_checkpoints_other_functions_removable_parts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_removable_parts_comment children_safety_results_general_checkpoints_other_functions_removable_parts_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_reversibility children_safety_results_general_checkpoints_other_functions_reversibility { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_reversibility_comment children_safety_results_general_checkpoints_other_functions_reversibility_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>

        #endregion

        #region CWf
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_fw22 children_safety_results_general_checkpoints_care_label_length_care_label_length_fw22 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_fw22_comment children_safety_results_general_checkpoints_care_label_length_care_label_length_fw22_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss22 children_safety_results_general_checkpoints_care_label_length_care_label_length_ss22 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss22_comment children_safety_results_general_checkpoints_care_label_length_care_label_length_ss22_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fasten children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fasten { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fasten_comment children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fasten_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fat children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fat_comment children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fat_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_hazardous_polution children_safety_results_general_checkpoints_general_safety_hazardous_polution { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_general_safety_hazardous_polution_comment children_safety_results_general_checkpoints_general_safety_hazardous_polution_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_hoodie children_safety_results_general_checkpoints_other_functions_hoodie { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_other_functions_hoodie_comment children_safety_results_general_checkpoints_other_functions_hoodie_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_textile_attached_clothing children_safety_results_general_checkpoints_textile_attached_clothing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_textile_attached_clothing_comment children_safety_results_general_checkpoints_textile_attached_clothing_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_total_circumference_7_5 children_safety_results_general_checkpoints_total_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_total_circumference_7_5_comment children_safety_results_general_checkpoints_total_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_length_7_5 children_safety_results_general_checkpoints_zipper_puller_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_length_7_5_comment children_safety_results_general_checkpoints_zipper_puller_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_cuff children_safety_results_general_checkpoints_zipper_puller_not_fall_below_cuff { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_cuff_comment children_safety_results_general_checkpoints_zipper_puller_not_fall_below_cuff_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_hem children_safety_results_general_checkpoints_zipper_puller_not_fall_below_hem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_hem_comment children_safety_results_general_checkpoints_zipper_puller_not_fall_below_hem_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_not_fall_more_than_1 children_safety_results_general_checkpoints_zipper_puller_not_fall_more_than_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_general_checkpoints_zipper_puller_not_fall_more_than_1_comment children_safety_results_general_checkpoints_zipper_puller_not_fall_more_than_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_buttons_forbidden children_safety_results_zone_a_adjusting_tab_both_age_buttons_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_buttons_forbidden_comment children_safety_results_zone_a_adjusting_tab_both_age_buttons_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_china_forbidden children_safety_results_zone_a_adjusting_tab_both_age_china_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_china_forbidden_comment children_safety_results_zone_a_adjusting_tab_both_age_china_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_height_2 children_safety_results_zone_a_adjusting_tab_both_age_height_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_adjusting_tab_both_age_height_2_comment children_safety_results_zone_a_adjusting_tab_both_age_height_2_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_decorative_functional_cord_free_end_20 children_safety_results_zone_a_bibs_and_hats_both_age_decorative_functional_cord_free_end_20 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_decorative_functional_cord_free_end_20_comment children_safety_results_zone_a_bibs_and_hats_both_age_decorative_functional_cord_free_end_20_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_drawstring_forbidden children_safety_results_zone_a_bibs_and_hats_both_age_drawstring_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_drawstring_forbidden_comment children_safety_results_zone_a_bibs_and_hats_both_age_drawstring_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_tab_length_20 children_safety_results_zone_a_bibs_and_hats_both_age_tab_length_20 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_tab_length_20_comment children_safety_results_zone_a_bibs_and_hats_both_age_tab_length_20_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_us_market_only_velcro_allowed children_safety_results_zone_a_bibs_and_hats_both_age_us_market_only_velcro_allowed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_bibs_and_hats_both_age_us_market_only_velcro_allowed_comment children_safety_results_zone_a_bibs_and_hats_both_age_us_market_only_velcro_allowed_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_decorative_cord_fixed children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_decorative_cord_fixed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_decorative_cord_fixed_comment children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_decorative_cord_fixed_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_7_5 children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_7_5_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_throat children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_throat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_throat_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_throat_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_knots_forbidden children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_knots_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_knots_forbidden_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_knots_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_loop_circumference_7_5 children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_loop_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_loop_circumference_7_5_comment children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_loop_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_cord_length_7_5 children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_cord_length_7_5_comment children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_flat_loop_length_7_5 children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_flat_loop_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_flat_loop_length_7_5_comment children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_flat_loop_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_loop_circumference_7_5 children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_loop_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_loop_circumference_7_5_comment children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_loop_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_other_parts_elastic_cord children_safety_results_zone_a_decorative_cord_8_14_years_other_parts_elastic_cord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_decorative_cord_8_14_years_other_parts_elastic_cord_comment children_safety_results_zone_a_decorative_cord_8_14_years_other_parts_elastic_cord_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_blocking_part_fixed children_safety_results_zone_a_drawstring_8_14_years_blocking_part_fixed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_blocking_part_fixed_comment children_safety_results_zone_a_drawstring_8_14_years_blocking_part_fixed_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_no_free_ends children_safety_results_zone_a_drawstring_8_14_years_no_free_ends { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_no_free_ends_comment children_safety_results_zone_a_drawstring_8_14_years_no_free_ends_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_no_protruding children_safety_results_zone_a_drawstring_8_14_years_no_protruding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_no_protruding_comment children_safety_results_zone_a_drawstring_8_14_years_no_protruding_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_protruding_loop_15 children_safety_results_zone_a_drawstring_8_14_years_protruding_loop_15 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_drawstring_8_14_years_protruding_loop_15_comment children_safety_results_zone_a_drawstring_8_14_years_protruding_loop_15_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_duffle_coat_buttons_0_7_years_forbidden children_safety_results_zone_a_duffle_coat_buttons_0_7_years_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_duffle_coat_buttons_0_7_years_forbidden_comment children_safety_results_zone_a_duffle_coat_buttons_0_7_years_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_duffle_coat_buttons_8_14_years_loop_circumference_7_5 children_safety_results_zone_a_duffle_coat_buttons_8_14_years_loop_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_duffle_coat_buttons_8_14_years_loop_circumference_7_5_comment children_safety_results_zone_a_duffle_coat_buttons_8_14_years_loop_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_ears_on_hood_both_age_ear_length_7_5 children_safety_results_zone_a_ears_on_hood_both_age_ear_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_ears_on_hood_both_age_ear_length_7_5_comment children_safety_results_zone_a_ears_on_hood_both_age_ear_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_ears_on_hood_both_age_not_able_to_tie children_safety_results_zone_a_ears_on_hood_both_age_not_able_to_tie { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_ears_on_hood_both_age_not_able_to_tie_comment children_safety_results_zone_a_ears_on_hood_both_age_not_able_to_tie_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_bows_forbidden children_safety_results_zone_a_functional_cord_8_14_years_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_bows_forbidden_comment children_safety_results_zone_a_functional_cord_8_14_years_bows_forbidden_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_free_end_7_5 children_safety_results_zone_a_functional_cord_8_14_years_free_end_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_functional_cord_8_14_years_free_end_7_5_comment children_safety_results_zone_a_functional_cord_8_14_years_free_end_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_closing_system_prohibited children_safety_results_zone_a_halter_neck_both_age_decorative_bow_closing_system_prohibited { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_closing_system_prohibited_comment children_safety_results_zone_a_halter_neck_both_age_decorative_bow_closing_system_prohibited_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_cords_flat_body children_safety_results_zone_a_halter_neck_both_age_decorative_bow_cords_flat_body { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_cords_flat_body_comment children_safety_results_zone_a_halter_neck_both_age_decorative_bow_cords_flat_body_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_forbidden children_safety_results_zone_a_halter_neck_both_age_decorative_bow_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_forbidden_comment children_safety_results_zone_a_halter_neck_both_age_decorative_bow_forbidden_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_elastic_cord children_safety_results_zone_a_loops_inside_both_age_elastic_cord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_elastic_cord_comment children_safety_results_zone_a_loops_inside_both_age_elastic_cord_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_loop_flat children_safety_results_zone_a_loops_inside_both_age_loop_flat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_loop_flat_comment children_safety_results_zone_a_loops_inside_both_age_loop_flat_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_loop_height_1 children_safety_results_zone_a_loops_inside_both_age_loop_height_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_loop_height_1_comment children_safety_results_zone_a_loops_inside_both_age_loop_height_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_loop_length_4 children_safety_results_zone_a_loops_inside_both_age_loop_length_4 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_loops_inside_both_age_loop_length_4_comment children_safety_results_zone_a_loops_inside_both_age_loop_length_4_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_neck_back_loop_both_age_buckle_circumference children_safety_results_zone_a_neck_back_loop_both_age_buckle_circumference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_neck_back_loop_both_age_buckle_circumference_comment children_safety_results_zone_a_neck_back_loop_both_age_buckle_circumference_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loop_circumference_7_5 children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loop_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loop_circumference_7_5_comment children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loop_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_free_end_7_5 children_safety_results_zone_a_shoulder_strap_0_7_years_free_end_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_free_end_7_5_comment children_safety_results_zone_a_shoulder_strap_0_7_years_free_end_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_1 children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_1_comment children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_must_be_flat children_safety_results_zone_a_shoulder_strap_0_7_years_must_be_flat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_0_7_years_must_be_flat_comment children_safety_results_zone_a_shoulder_strap_0_7_years_must_be_flat_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_8_14_years_free_end_7_5 children_safety_results_zone_a_shoulder_strap_8_14_years_free_end_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_8_14_years_free_end_7_5_comment children_safety_results_zone_a_shoulder_strap_8_14_years_free_end_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_fixed_loop_circumference_7_5 children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_fixed_loop_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_fixed_loop_circumference_7_5_comment children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_fixed_loop_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_free_end_7_5 children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_free_end_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_free_end_7_5_comment children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_free_end_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_protective_tape_mandatory children_safety_results_zone_a_zip_protection_protective_tabs_both_age_protective_tape_mandatory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_protective_tape_mandatory_comment children_safety_results_zone_a_zip_protection_protective_tabs_both_age_protective_tape_mandatory_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_stop_before_top children_safety_results_zone_a_zip_protection_protective_tabs_both_age_stop_before_top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_stop_before_top_comment children_safety_results_zone_a_zip_protection_protective_tabs_both_age_stop_before_top_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_adjusting_tabs children_safety_results_zone_b_adjusting_tab_both_age_adjusting_tabs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_adjusting_tabs_comment children_safety_results_zone_b_adjusting_tab_both_age_adjusting_tabs_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_buttons_permitted children_safety_results_zone_b_adjusting_tab_both_age_buttons_permitted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_buttons_permitted_comment children_safety_results_zone_b_adjusting_tab_both_age_buttons_permitted_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_edge_jacket_not_hanging children_safety_results_zone_b_adjusting_tab_both_age_edge_jacket_not_hanging { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_edge_jacket_not_hanging_comment children_safety_results_zone_b_adjusting_tab_both_age_edge_jacket_not_hanging_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_height_2 children_safety_results_zone_b_adjusting_tab_both_age_height_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_height_2_comment children_safety_results_zone_b_adjusting_tab_both_age_height_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_tab_length_14 children_safety_results_zone_b_adjusting_tab_both_age_tab_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_adjusting_tab_both_age_tab_length_14_comment children_safety_results_zone_b_adjusting_tab_both_age_tab_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_cord_length_7_5 children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_cord_length_7_5_comment children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_length_7_5 children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_length_7_5_comment children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_teeshirt_loose_end_front_length_14 children_safety_results_zone_b_decorative_cord_both_age_teeshirt_loose_end_front_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_decorative_cord_both_age_teeshirt_loose_end_front_length_14_comment children_safety_results_zone_b_decorative_cord_both_age_teeshirt_loose_end_front_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_bows_forbidden children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_bows_forbidden_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_cords_hang_lower_garment children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_cords_hang_lower_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_cords_hang_lower_garment_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_cords_hang_lower_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_free_end_7_5 children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_free_end_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_free_end_7_5_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_free_end_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_security_stitch children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_security_stitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_security_stitch_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_security_stitch_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_bows_forbidden children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_bows_forbidden_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_cords_hang_lower_garment children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_cords_hang_lower_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_cords_hang_lower_garment_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_cords_hang_lower_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_free_end_7_5 children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_free_end_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_free_end_7_5_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_free_end_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_security_stitch children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_security_stitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_security_stitch_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_security_stitch_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_bows_forbidden children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_bows_forbidden_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_cords_hang_lower_garment children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_cords_hang_lower_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_cords_hang_lower_garment_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_cords_hang_lower_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_14 children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_14_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_7_5 children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_7_5_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_number_belt_loop children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_number_belt_loop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_number_belt_loop_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_number_belt_loop_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_security_stitch children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_security_stitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_security_stitch_comment children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_security_stitch_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_elasticated_button_holes_must_have_fixed_bartack children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_elasticated_button_holes_must_have_fixed_bartack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_elasticated_button_holes_must_have_fixed_bartack_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_elasticated_button_holes_must_have_fixed_bartack_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_adjustable_blocking_fixed children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_adjustable_blocking_fixed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_adjustable_blocking_fixed_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_adjustable_blocking_fixed_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_cords_hang children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_cords_hang { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_cords_hang_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_cords_hang_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_free_ends children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_free_ends { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_free_ends_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_free_ends_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_front children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_front { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_front_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_front_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_natural children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_natural { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_natural_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_natural_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_opened children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_opened { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_opened_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_opened_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_security_stitch children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_security_stitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_security_stitch_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_security_stitch_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_adjustable_blocking_devices_fixed children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_adjustable_blocking_devices_fixed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_adjustable_blocking_devices_fixed_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_adjustable_blocking_devices_fixed_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_protruding_loops_forbidden children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_protruding_loops_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_protruding_loops_forbidden_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_protruding_loops_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_security_stitch children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_security_stitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_security_stitch_comment children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_security_stitch_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_kont_prohibited children_safety_results_zone_b_fixed_decorative_knot_both_age_kont_prohibited { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_kont_prohibited_comment children_safety_results_zone_b_fixed_decorative_knot_both_age_kont_prohibited_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_circumference_7_5 children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_circumference_7_5_comment children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_length_14 children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_length_14_comment children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_free_end_14 children_safety_results_zone_b_fixed_decorative_knot_both_age_max_free_end_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_free_end_14_comment children_safety_results_zone_b_fixed_decorative_knot_both_age_max_free_end_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fringes_both_age_max_length_14 children_safety_results_zone_b_fringes_both_age_max_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fringes_both_age_max_length_14_comment children_safety_results_zone_b_fringes_both_age_max_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fringes_both_age_not_hang_lower children_safety_results_zone_b_fringes_both_age_not_hang_lower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_fringes_both_age_not_hang_lower_comment children_safety_results_zone_b_fringes_both_age_not_hang_lower_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_bows_forbidden children_safety_results_zone_b_functional_cord_both_age_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_bows_forbidden_comment children_safety_results_zone_b_functional_cord_both_age_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_cord_length_7_5 children_safety_results_zone_b_functional_cord_both_age_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_functional_cord_both_age_cord_length_7_5_comment children_safety_results_zone_b_functional_cord_both_age_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_length_36 children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_length_36 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_length_36_comment children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_length_36_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_width_tying_point_3 children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_width_tying_point_3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_width_tying_point_3_comment children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_width_tying_point_3_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_bows_forbidden children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_bows_forbidden_comment children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_max_1 children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_max_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_max_1_comment children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_max_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_not_longer_bottom_garment children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_not_longer_bottom_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_not_longer_bottom_garment_comment children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_not_longer_bottom_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_buttons_forbidden children_safety_results_zone_c_adjusting_tab_both_age_buttons_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_buttons_forbidden_comment children_safety_results_zone_c_adjusting_tab_both_age_buttons_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_garment children_safety_results_zone_c_adjusting_tab_both_age_not_hang_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_garment_comment children_safety_results_zone_c_adjusting_tab_both_age_not_hang_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_22 children_safety_results_zone_c_adjusting_tab_both_age_tab_length_22 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_22_comment children_safety_results_zone_c_adjusting_tab_both_age_tab_length_22_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_width_2 children_safety_results_zone_c_adjusting_tab_both_age_width_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_adjusting_tab_both_age_width_2_comment children_safety_results_zone_c_adjusting_tab_both_age_width_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_bottom_legs_tab_both_age_tab_not_below_garment children_safety_results_zone_c_bottom_legs_tab_both_age_tab_not_below_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_bottom_legs_tab_both_age_tab_not_below_garment_comment children_safety_results_zone_c_bottom_legs_tab_both_age_tab_not_below_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_ankle_length_clothing_cords_not_outside_garment children_safety_results_zone_c_drawstrings_both_age_ankle_length_clothing_cords_not_outside_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_ankle_length_clothing_cords_not_outside_garment_comment children_safety_results_zone_c_drawstrings_both_age_ankle_length_clothing_cords_not_outside_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_bows_forbidden children_safety_results_zone_c_drawstrings_both_age_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_bows_forbidden_comment children_safety_results_zone_c_drawstrings_both_age_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_cord_length_7_5 children_safety_results_zone_c_drawstrings_both_age_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_cord_length_7_5_comment children_safety_results_zone_c_drawstrings_both_age_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_fringes_added_on_garment_not_hang children_safety_results_zone_c_drawstrings_both_age_fringes_added_on_garment_not_hang { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_fringes_added_on_garment_not_hang_comment children_safety_results_zone_c_drawstrings_both_age_fringes_added_on_garment_not_hang_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_fringes_ankles_fringes_max_length_2 children_safety_results_zone_c_drawstrings_both_age_fringes_ankles_fringes_max_length_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_fringes_ankles_fringes_max_length_2_comment children_safety_results_zone_c_drawstrings_both_age_fringes_ankles_fringes_max_length_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_fringes_fringes_max_length_7_5 children_safety_results_zone_c_drawstrings_both_age_fringes_fringes_max_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_fringes_fringes_max_length_7_5_comment children_safety_results_zone_c_drawstrings_both_age_fringes_fringes_max_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_garments_hanging_not_hang children_safety_results_zone_c_drawstrings_both_age_garments_hanging_not_hang { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_garments_hanging_not_hang_comment children_safety_results_zone_c_drawstrings_both_age_garments_hanging_not_hang_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_lower_edge_garments_cords_against_garment children_safety_results_zone_c_drawstrings_both_age_lower_edge_garments_cords_against_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_lower_edge_garments_cords_against_garment_comment children_safety_results_zone_c_drawstrings_both_age_lower_edge_garments_cords_against_garment_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_no_protruding_loop children_safety_results_zone_c_drawstrings_both_age_no_protruding_loop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_c_drawstrings_both_age_no_protruding_loop_comment children_safety_results_zone_c_drawstrings_both_age_no_protruding_loop_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_buttons_forbidden children_safety_results_zone_d_adjusting_tab_both_age_buttons_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_buttons_forbidden_comment children_safety_results_zone_d_adjusting_tab_both_age_buttons_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_garment children_safety_results_zone_d_adjusting_tab_both_age_not_hang_garment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_garment_comment children_safety_results_zone_d_adjusting_tab_both_age_not_hang_garment_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_belt_loop_both_age_belt_loop_max_1 children_safety_results_zone_d_belt_loop_both_age_belt_loop_max_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_belt_loop_both_age_belt_loop_max_1_comment children_safety_results_zone_d_belt_loop_both_age_belt_loop_max_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_bows_knots_forbidden children_safety_results_zone_d_decorative_cord_both_age_bows_knots_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_bows_knots_forbidden_comment children_safety_results_zone_d_decorative_cord_both_age_bows_knots_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_cord_length_length_7_5 children_safety_results_zone_d_decorative_cord_both_age_cord_length_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_cord_length_length_7_5_comment children_safety_results_zone_d_decorative_cord_both_age_cord_length_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_decorative_cords_forbidden children_safety_results_zone_d_decorative_cord_both_age_decorative_cords_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_decorative_cords_forbidden_comment children_safety_results_zone_d_decorative_cord_both_age_decorative_cords_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_circumference_7_5 children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_circumference_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_circumference_7_5_comment children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_circumference_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_securing_stitch children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_securing_stitch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_securing_stitch_comment children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_securing_stitch_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_both_age_forbidden children_safety_results_zone_d_drawstring_functional_cords_both_age_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_drawstring_functional_cords_both_age_forbidden_comment children_safety_results_zone_d_drawstring_functional_cords_both_age_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_martingales_both_age_length_fixed_pioints_7_5 children_safety_results_zone_d_martingales_both_age_length_fixed_pioints_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_martingales_both_age_length_fixed_pioints_7_5_comment children_safety_results_zone_d_martingales_both_age_length_fixed_pioints_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_martingales_both_age_width_2 children_safety_results_zone_d_martingales_both_age_width_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_martingales_both_age_width_2_comment children_safety_results_zone_d_martingales_both_age_width_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_0_7_years_not_hang_lower children_safety_results_zone_d_tied_belt_sashes_bow_ties_0_7_years_not_hang_lower { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_0_7_years_not_hang_lower_comment children_safety_results_zone_d_tied_belt_sashes_bow_ties_0_7_years_not_hang_lower_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_8_14_years_belt_not_attached children_safety_results_zone_d_tied_belt_sashes_bow_ties_8_14_years_belt_not_attached { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_8_14_years_belt_not_attached_comment children_safety_results_zone_d_tied_belt_sashes_bow_ties_8_14_years_belt_not_attached_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_length_36 children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_length_36 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_length_36_comment children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_length_36_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_width_3 children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_width_3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_width_3_comment children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_width_3_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_max_1 children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_max_1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_max_1_comment children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_max_1_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_ornaments_forbidden children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_ornaments_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_ornaments_forbidden_comment children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_ornaments_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_adjusting_tab_both_age_not_hang children_safety_results_zone_e_adjusting_tab_both_age_not_hang { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_adjusting_tab_both_age_not_hang_comment children_safety_results_zone_e_adjusting_tab_both_age_not_hang_comment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_gloves_both_age_buttons_forbidden children_safety_results_zone_e_gloves_both_age_buttons_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_gloves_both_age_buttons_forbidden_comment children_safety_results_zone_e_gloves_both_age_buttons_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_gloves_both_age_gloves_drawstring_gloves_come_easily children_safety_results_zone_e_gloves_both_age_gloves_drawstring_gloves_come_easily { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_gloves_both_age_gloves_drawstring_gloves_come_easily_comment children_safety_results_zone_e_gloves_both_age_gloves_drawstring_gloves_come_easily_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_decorative_above_elbow_cord_length_7_5 children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_decorative_above_elbow_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_decorative_above_elbow_cord_length_7_5_comment children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_decorative_above_elbow_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_functional_decorative_above_elbow_cord_length_14 children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_functional_decorative_above_elbow_cord_length_14 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_functional_decorative_above_elbow_cord_length_14_comment children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_functional_decorative_above_elbow_cord_length_14_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_bows_forbidden children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_bows_forbidden_comment children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_below_elbow_cord_length_7_5 children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_below_elbow_cord_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_below_elbow_cord_length_7_5_comment children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_below_elbow_cord_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_lower_edge_cords_outside_forbidden children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_lower_edge_cords_outside_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_lower_edge_cords_outside_forbidden_comment children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_lower_edge_cords_outside_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_tabs_inside_sleeve_bows_forbidden children_safety_results_zone_e_long_sleeves_both_age_tabs_inside_sleeve_bows_forbidden { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_long_sleeves_both_age_tabs_inside_sleeve_bows_forbidden_comment children_safety_results_zone_e_long_sleeves_both_age_tabs_inside_sleeve_bows_forbidden_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_0_7_years_sleeve_below_elbow_tab_length_7_5 children_safety_results_zone_e_rollup_sleeve_tab_0_7_years_sleeve_below_elbow_tab_length_7_5 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_0_7_years_sleeve_below_elbow_tab_length_7_5_comment children_safety_results_zone_e_rollup_sleeve_tab_0_7_years_sleeve_below_elbow_tab_length_7_5_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_8_14_years_sleeve_below_elbow_tab_length_10 children_safety_results_zone_e_rollup_sleeve_tab_8_14_years_sleeve_below_elbow_tab_length_10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_8_14_years_sleeve_below_elbow_tab_length_10_comment children_safety_results_zone_e_rollup_sleeve_tab_8_14_years_sleeve_below_elbow_tab_length_10_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_fixed_above children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_fixed_above { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_fixed_above_comment children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_fixed_above_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_length_10 children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_length_10 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_length_10_comment children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_length_10_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2 children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_comment children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_sleeve_above_elbow children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_sleeve_above_elbow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_sleeve_above_elbow_comment children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_sleeve_above_elbow_comment { get; set; }
        #endregion
    }



    public class Children_safety_results_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    #region update for General safety
    public class Children_safety_results_children_safety_visible
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_visible
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_button_pulling_test
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_button_pulling_test_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_hazardous_pollution
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_hazardous_pollution_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_needle_book
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_needle_book_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_needle_detection
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_needle_detection_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_others
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_others_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_small_parts_pulling_test
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_small_parts_pulling_test_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_warning_poly_bags
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_warning_poly_bags_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_zipper_pulling_test
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_safety_zipper_pulling_test_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_photos_children_safety
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Children_safety_results_photos_general_safety
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }
    #endregion

    #region update for safety-> General  checkpoints
    public class Children_safety_results_general_checkpoints_functionality_buckle
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_buckle_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_button
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_button_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_hooks
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_hooks_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_other_closures
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_other_closures_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_velcro
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_velcro_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_zipper
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_functionality_zipper_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_hazardous_pollution
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_hazardous_pollution_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_needle_book
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_needle_book_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_needle_detection
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_needle_detection_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_others
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_others_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_pulling_test
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_pulling_test_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_warning_polybag
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_warning_polybag_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_head_opening
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_head_opening_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_hoodie_garage
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_hoodie_garage_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_others
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_others_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_removable_parts
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_removable_parts_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_reversibility
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_reversibility_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    #endregion

    #region cwf children safety
    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_fw22
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_fw22_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss22
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss22_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fasten
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fasten_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fat
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_care_label_length_care_label_length_ss23_fat_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Children_safety_results_general_checkpoints_general_safety_hazardous_polution
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_general_safety_hazardous_polution_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_general_checkpoints_other_functions_hoodie
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_other_functions_hoodie_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Children_safety_results_general_checkpoints_textile_attached_clothing
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_textile_attached_clothing_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_total_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_total_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_cuff
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_cuff_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_hem
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_not_fall_below_hem_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_not_fall_more_than_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_general_checkpoints_zipper_puller_not_fall_more_than_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_buttons_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_buttons_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_china_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_china_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_height_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_adjusting_tab_both_age_height_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_bibs_and_hats_both_age_decorative_functional_cord_free_end_20
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_bibs_and_hats_both_age_decorative_functional_cord_free_end_20_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_bibs_and_hats_both_age_drawstring_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_bibs_and_hats_both_age_drawstring_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_bibs_and_hats_both_age_tab_length_20
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_bibs_and_hats_both_age_tab_length_20_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_bibs_and_hats_both_age_us_market_only_velcro_allowed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_bibs_and_hats_both_age_us_market_only_velcro_allowed_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_decorative_cord_fixed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_hood_back_neck_decorative_cord_fixed_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_throat
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_cord_length_throat_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_knots_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_knots_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_loop_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_0_7_years_other_parts_loop_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_flat_loop_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_flat_loop_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_loop_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_hood_back_neck_loop_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_other_parts_elastic_cord
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_decorative_cord_8_14_years_other_parts_elastic_cord_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_drawstring_8_14_years_blocking_part_fixed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_blocking_part_fixed_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_drawstring_8_14_years_no_free_ends
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_no_free_ends_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_no_protruding
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_no_protruding_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_protruding_loop_15
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_drawstring_8_14_years_protruding_loop_15_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_duffle_coat_buttons_0_7_years_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_duffle_coat_buttons_0_7_years_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_duffle_coat_buttons_8_14_years_loop_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_duffle_coat_buttons_8_14_years_loop_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_ears_on_hood_both_age_ear_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_ears_on_hood_both_age_ear_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_ears_on_hood_both_age_not_able_to_tie
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_ears_on_hood_both_age_not_able_to_tie_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_functional_cord_8_14_years_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_functional_cord_8_14_years_free_end_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_functional_cord_8_14_years_free_end_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_closing_system_prohibited
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_closing_system_prohibited_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_cords_flat_body
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_cords_flat_body_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_halter_neck_both_age_decorative_bow_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_a_loops_inside_both_age_elastic_cord
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_loops_inside_both_age_elastic_cord_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_loops_inside_both_age_loop_flat
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_loops_inside_both_age_loop_flat_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_loops_inside_both_age_loop_height_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_loops_inside_both_age_loop_height_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_loops_inside_both_age_loop_length_4
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_loops_inside_both_age_loop_length_4_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_neck_back_loop_both_age_buckle_circumference
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_neck_back_loop_both_age_buckle_circumference_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loop_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_fixed_loop_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_free_end_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_free_end_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_free_ends_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_must_be_flat
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_0_7_years_must_be_flat_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_8_14_years_free_end_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_8_14_years_free_end_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_fixed_loop_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_fixed_loop_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_free_end_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_shoulder_strap_both_age_fixed_cord_attached_to_strap_free_end_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_protective_tape_mandatory
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_protective_tape_mandatory_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_stop_before_top
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_a_zip_protection_protective_tabs_both_age_stop_before_top_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_adjusting_tabs
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_adjusting_tabs_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_buttons_permitted
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_buttons_permitted_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_edge_jacket_not_hanging
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_edge_jacket_not_hanging_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_height_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_height_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_tab_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_adjusting_tab_both_age_tab_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_dress_loose_end_front_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_teeshirt_loose_end_front_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_decorative_cord_both_age_teeshirt_loose_end_front_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_cords_hang_lower_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_cords_hang_lower_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_free_end_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_free_end_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_security_stitch
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_coats_security_stitch_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_cords_hang_lower_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_cords_hang_lower_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_free_end_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_free_end_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_security_stitch
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_shirts_security_stitch_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_cords_hang_lower_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_cords_hang_lower_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_free_end_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_number_belt_loop
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_number_belt_loop_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_security_stitch
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_with_free_ends_trousers_security_stitch_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_elasticated_button_holes_must_have_fixed_bartack
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_elasticated_button_holes_must_have_fixed_bartack_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_adjustable_blocking_fixed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_adjustable_blocking_fixed_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_cords_hang
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_cords_hang_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_free_ends
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_free_ends_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_front
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_front_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_natural
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_natural_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_opened
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_no_protruding_garment_opened_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_security_stitch
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_inside_garment_shirt_coat_parka_security_stitch_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_adjustable_blocking_devices_fixed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_adjustable_blocking_devices_fixed_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_protruding_loops_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_protruding_loops_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_security_stitch
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_drawstring_both_age_without_free_ends_outside_garment_all_products_security_stitch_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_kont_prohibited
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_kont_prohibited_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_bow_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_free_end_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fixed_decorative_knot_both_age_max_free_end_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fringes_both_age_max_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fringes_both_age_max_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fringes_both_age_not_hang_lower
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_fringes_both_age_not_hang_lower_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_functional_cord_both_age_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_length_36
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_length_36_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_width_tying_point_3
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_belt_width_tying_point_3_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_max_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_max_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_not_longer_bottom_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_b_tied_belt_sashes_bow_ties_both_age_end_belt_not_longer_bottom_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_buttons_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_buttons_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_not_hang_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_22
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_tab_length_22_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_width_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_adjusting_tab_both_age_width_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_bottom_legs_tab_both_age_tab_not_below_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_bottom_legs_tab_both_age_tab_not_below_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_ankle_length_clothing_cords_not_outside_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_ankle_length_clothing_cords_not_outside_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_fringes_added_on_garment_not_hang
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_fringes_added_on_garment_not_hang_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_fringes_ankles_fringes_max_length_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_fringes_ankles_fringes_max_length_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_fringes_fringes_max_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_fringes_fringes_max_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_garments_hanging_not_hang
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_garments_hanging_not_hang_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_lower_edge_garments_cords_against_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_lower_edge_garments_cords_against_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_no_protruding_loop
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_c_drawstrings_both_age_no_protruding_loop_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_buttons_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_buttons_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_garment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_adjusting_tab_both_age_not_hang_garment_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Children_safety_results_zone_d_belt_loop_both_age_belt_loop_max_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_belt_loop_both_age_belt_loop_max_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_bows_knots_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_bows_knots_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_cord_length_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_cord_length_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_decorative_cords_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_decorative_cords_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_circumference_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_circumference_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_securing_stitch
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_decorative_cord_both_age_fixed_loops_securing_stitch_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_both_age_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_drawstring_functional_cords_both_age_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_martingales_both_age_length_fixed_pioints_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_martingales_both_age_length_fixed_pioints_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_martingales_both_age_width_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_martingales_both_age_width_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_0_7_years_not_hang_lower
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_0_7_years_not_hang_lower_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_8_14_years_belt_not_attached
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_8_14_years_belt_not_attached_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_length_36
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_length_36_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_width_3
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_belt_width_3_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_max_1
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_max_1_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_ornaments_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_d_tied_belt_sashes_bow_ties_both_age_end_belt_ornaments_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_adjusting_tab_both_age_not_hang
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_adjusting_tab_both_age_not_hang_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Children_safety_results_zone_e_gloves_both_age_buttons_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_gloves_both_age_buttons_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_gloves_both_age_gloves_drawstring_gloves_come_easily
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_gloves_both_age_gloves_drawstring_gloves_come_easily_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_decorative_above_elbow_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_0_7_years_drawstring_functional_decorative_above_elbow_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_functional_decorative_above_elbow_cord_length_14
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_8_14_years_drawstring_functional_decorative_above_elbow_cord_length_14_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_below_elbow_cord_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_below_elbow_cord_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_lower_edge_cords_outside_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_drawstring_functional_decorative_lower_edge_cords_outside_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_tabs_inside_sleeve_bows_forbidden
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_long_sleeves_both_age_tabs_inside_sleeve_bows_forbidden_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_0_7_years_sleeve_below_elbow_tab_length_7_5
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_0_7_years_sleeve_below_elbow_tab_length_7_5_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_8_14_years_sleeve_below_elbow_tab_length_10
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_8_14_years_sleeve_below_elbow_tab_length_10_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_fixed_above
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_fixed_above_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_length_10
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_tab_length_10_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_sleeve_above_elbow
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Children_safety_results_zone_e_rollup_sleeve_tab_both_age_width_2_sleeve_above_elbow_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    #endregion 

    public class Children_safety_results
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results_fields children_safety_results_fields { get; set; }
    }

    public class Children_safety_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Children_safety_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_photos_photos children_safety_photos_photos { get; set; }
    }

    public class Children_safety_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_photos_fields children_safety_photos_fields { get; set; }
    }

    public class Children_safety_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_results children_safety_results { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_photos children_safety_photos { get; set; }
    }

    public class Children_safety
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_fields children_safety_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety_subSections children_safety_subSections { get; set; }
    }

    public class Appendix_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Appendix_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Appendix_photos_photos appendix_photos_photos { get; set; }
    }

    public class Appendix_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Appendix_photos_fields appendix_photos_fields { get; set; }
    }

    public class Appendix_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Appendix_photos appendix_photos { get; set; }
    }
    public class Appendix_fields
    {

    }
    public class Appendix
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Appendix_fields appendix_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Appendix_subSections appendix_subSections { get; set; }
    }

    public class Sections
    {
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia frontpage_studio_asia { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Result_and_conclusion result_and_conclusion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship workmanship { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Workmanship2 workmanship2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Measurement_and_fitting measurement_and_fitting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Conformity conformity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Packing_labeling packing_labeling { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Functionality functionality { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Children_safety children_safety { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests onsite_tests { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check color_shading_check { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Appendix appendix { get; set; }

        #region Fabric
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary fabric_detailed_summary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification fabric_defects_classification { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test fabric_rubbing_test { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Additional_photos additional_photos { get; set; }
        #endregion
    }

    public class Result_and_conclusion_amcharts_garment_grade_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_amcharts_garment_grade
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_amcharts_garment_grade_value> result_and_conclusion_amcharts_garment_grade_value { get; set; }
    }

    public class OrderDetailRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public Template template { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Products> products { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Sections sections { get; set; }
    }

    #region kith 100
    public class Result_and_conclusion_presented_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_measured_qty2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_measured_qty2_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_measurements_out_tolerance_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_measurements_out_tolerance_percent2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_number_pcs_measurements_out_tolerance2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inspected_qty2
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inspected_qty2_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_presented_percent_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    #endregion 

    #region Color_shading_check
    public class Color_shading_check_action_taken
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Color_shading_check_overview_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_action_taken color_shading_check_action_taken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_overall_result color_shading_check_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_overview_comments color_shading_check_overview_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_overview_result color_shading_check_overview_result { get; set; }
    }

    public class Color_shading_check_overall_result_totals
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_overall_result_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_overall_result_totals color_shading_check_overall_result_totals { get; set; }
    }

    public class Color_shading_check_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_overall_result_fields color_shading_check_overall_result_fields { get; set; }
    }

    public class Color_shading_check_results_lines_color_code
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_grade
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_lot
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_percentage_sampling
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_tone
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_compared_to_what
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_found
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_within_reference
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Color_shading_check_results_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_color_code color_shading_check_results_lines_color_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_grade color_shading_check_results_lines_grade { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_lot color_shading_check_results_lines_lot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_percentage_sampling color_shading_check_results_lines_percentage_sampling { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_tone color_shading_check_results_lines_tone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_compared_to_what color_shading_check_results_lines_compared_to_what { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_found color_shading_check_results_lines_found { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines_within_reference color_shading_check_results_lines_within_reference { get; set; }
    }

    public class Color_shading_check_results_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Color_shading_check_results_lines_lines> color_shading_check_results_lines_lines { get; set; }
    }

    public class Color_shading_check_results_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_lines color_shading_check_results_lines { get; set; }
    }

    public class Color_shading_check_results
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results_fields color_shading_check_results_fields { get; set; }
    }

    public class Color_shading_check_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_overall_result color_shading_check_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_results color_shading_check_results { get; set; }
    }

    public class Color_shading_check
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_fields color_shading_check_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color_shading_check_subSections color_shading_check_subSections { get; set; }
    }
    #endregion 

    #region add for inline
    public class Result_and_conclusion_cut_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_cut_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_finished_sewing_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_finished_sewing_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_inprocess_packing_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_inprocess_packing_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_inprocess_sewing_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_inprocess_sewing_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }
    public class Result_and_conclusion_packed_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_packed_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_total_cut_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_cut_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_cut_qty_details_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_finished_sewing_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_finished_sewing_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_finished_sewing_qty_details_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_total_inprocess_packing_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inprocess_packing_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inprocess_packing_qty_details_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inprocess_sewing_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inprocess_sewing_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inprocess_sewing_qty_details_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_total_packed_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_packed_qty_details_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_total_cut_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_finished_sewing_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inprocess_packing_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inprocess_sewing_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Workmanship_results_analysis_amcharts_defects_by_reparability_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int value { get; set; }
    }

    public class Workmanship_results_analysis_amcharts_defects_by_reparability
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Workmanship_results_analysis_amcharts_defects_by_reparability_value> workmanship_results_analysis_amcharts_defects_by_reparability_value { get; set; }
    }
    #endregion

    #region add for 100
    public class Result_and_conclusion_first_choice_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Result_and_conclusion_second_choice_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Result_and_conclusion_third_choice_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Result_and_conclusion_first_choice_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Result_and_conclusion_second_choice_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_third_choice_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }






    public class Workmanship_automatic_remark
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    #endregion 

    #region Fabric

    public class Result_and_conclusion_fabric_inspection_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Result_and_conclusion_length_unit
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_system_tolerance_roll
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_system_tolerance_shipment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_accepted_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_accepted_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Result_and_conclusion_total_produced_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_produced_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_rejected_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_rejected_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_rejected_qty_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_amcharts_fabric_inspection_detail_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? value { get; set; }
    }

    public class Result_and_conclusion_amcharts_fabric_inspection_detail
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_amcharts_fabric_inspection_detail_value> result_and_conclusion_amcharts_fabric_inspection_detail_value { get; set; }
    }

    public class Result_and_conclusion_amcharts_fabric_inspection_summary_value
    {
        /// <summary>
        /// 
        /// </summary>
        public string category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_amcharts_fabric_inspection_summary
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Result_and_conclusion_amcharts_fabric_inspection_summary_value> result_and_conclusion_amcharts_fabric_inspection_summary_value { get; set; }
    }

    public class Result_and_conclusion_approved_fabric_provided_by
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_aql_level
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_aql_level_custom
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Result_and_conclusion_arrival_date
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? value { get; set; }
    }

    public class Result_and_conclusion_color_conformity_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_color_conformity_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_color_fastness_rubbing_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_color_fastness_rubbing_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_customer_sealed_sample
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_departure_date
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? value { get; set; }
    }

    public class Result_and_conclusion_dyeing_defects_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_dyeing_defects_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_fabric_inspection_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_fabric_weight_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_fabric_weight_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_general_conformity_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_general_conformity_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_inspector_firstname
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_inspector_lastname
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_inspector_name
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_lab_dip_provided_by
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_nb_lots_presented
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_nb_rolls_presented
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_number_samples_sealed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_composition
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_fabric_code
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_parcel_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_report_date
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime? value { get; set; }
    }

    public class Result_and_conclusion_roll_packing_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_roll_packing_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_sgt_sealed_sample
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_accepted_qty_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_inspected_qty_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_presented_qty_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_total_produced_qty_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_warp_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_warp_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_weaving_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_weaving_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_weft_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_weft_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Fabric_detailed_summary_init_ordered_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_inspected_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_inspected_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_over_less_produced_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_produced_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_rating
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_rejected_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_rejected_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_tolerance
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_bowing_skewness
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_bulk_print_to_approved_print
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_dye_lots_to_dye_lots
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_dye_lots_to_lab_dip
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_edge_center_edge
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_embroidery
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_handfeel
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_joint_pieces_length
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_labeling_content
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_labeling_position
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_presented_to_packing_list
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_roll_length
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_roll_weight
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_rolls_to_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_inspected_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_inspected_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_over_less_produced_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_produced_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_rating
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_rejected_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_rejected_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_tolerance
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_weight
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_width
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_demerit_pts
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_bowing_skewness_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_bulk_print_to_approved_print_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_cleanliness
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_counter_of_inspection_machine
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_dye_lots_to_dye_lots_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_dye_lots_to_lab_dip_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_edge_center_edge_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_embroidery_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_handfeel_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_inspection_place
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_joint_pieces_length_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_labeling_content_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_labeling_position_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_light_box
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_lighting
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_presented_to_packing_list_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_roll_length_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_roll_weight_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_rolls_to_rolls_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_scale
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_total_demerit_pts
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_weight_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_width_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Result_and_conclusion_style_category
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_inspection_site
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_garment_vendor_factory
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_description_and_qty_of_sealed_samples
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Result_and_conclusion_parcel_awb_no
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_external_lab_used
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Fabric_detailed_summary_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_init_ordered_qty> fabric_detailed_summary_init_ordered_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_inspected_percent> fabric_detailed_summary_inspected_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_inspected_qty> fabric_detailed_summary_inspected_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_over_less_produced_qty> fabric_detailed_summary_over_less_produced_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_produced_qty> fabric_detailed_summary_produced_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_rating> fabric_detailed_summary_rating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_rejected_qty> fabric_detailed_summary_rejected_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_rejected_rolls> fabric_detailed_summary_rejected_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_tolerance> fabric_detailed_summary_tolerance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_bowing_skewness fabric_detailed_summary_bowing_skewness { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_bulk_print_to_approved_print fabric_detailed_summary_bulk_print_to_approved_print { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_dye_lots_to_dye_lots fabric_detailed_summary_dye_lots_to_dye_lots { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_dye_lots_to_lab_dip fabric_detailed_summary_dye_lots_to_lab_dip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_edge_center_edge fabric_detailed_summary_edge_center_edge { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_embroidery fabric_detailed_summary_embroidery { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_handfeel fabric_detailed_summary_handfeel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_joint_pieces_length fabric_detailed_summary_joint_pieces_length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_labeling_content fabric_detailed_summary_labeling_content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_labeling_position fabric_detailed_summary_labeling_position { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_presented_to_packing_list fabric_detailed_summary_presented_to_packing_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_roll_length fabric_detailed_summary_roll_length { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_roll_weight fabric_detailed_summary_roll_weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_rolls_to_rolls fabric_detailed_summary_rolls_to_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_inspected_percent fabric_detailed_summary_total_inspected_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_inspected_qty fabric_detailed_summary_total_inspected_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_over_less_produced_qty fabric_detailed_summary_total_over_less_produced_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_produced_qty fabric_detailed_summary_total_produced_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_rating fabric_detailed_summary_total_rating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_rejected_qty fabric_detailed_summary_total_rejected_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_rejected_rolls fabric_detailed_summary_total_rejected_rolls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_tolerance fabric_detailed_summary_total_tolerance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_weight fabric_detailed_summary_weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_width fabric_detailed_summary_width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_demerit_pts> fabric_detailed_summary_demerit_pts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_bowing_skewness_comment fabric_detailed_summary_bowing_skewness_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_bulk_print_to_approved_print_comment fabric_detailed_summary_bulk_print_to_approved_print_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_cleanliness fabric_detailed_summary_cleanliness { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_counter_of_inspection_machine fabric_detailed_summary_counter_of_inspection_machine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_dye_lots_to_dye_lots_comment fabric_detailed_summary_dye_lots_to_dye_lots_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_dye_lots_to_lab_dip_comment fabric_detailed_summary_dye_lots_to_lab_dip_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_edge_center_edge_comment fabric_detailed_summary_edge_center_edge_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_embroidery_comment fabric_detailed_summary_embroidery_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_handfeel_comment fabric_detailed_summary_handfeel_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_inspection_place fabric_detailed_summary_inspection_place { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_joint_pieces_length_comment fabric_detailed_summary_joint_pieces_length_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_labeling_content_comment fabric_detailed_summary_labeling_content_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_labeling_position_comment fabric_detailed_summary_labeling_position_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_light_box fabric_detailed_summary_light_box { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_lighting fabric_detailed_summary_lighting { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_presented_to_packing_list_comment fabric_detailed_summary_presented_to_packing_list_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_roll_length_comment fabric_detailed_summary_roll_length_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_roll_weight_comment fabric_detailed_summary_roll_weight_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_rolls_to_rolls_comment fabric_detailed_summary_rolls_to_rolls_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_scale fabric_detailed_summary_scale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_total_demerit_pts fabric_detailed_summary_total_demerit_pts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_weight_comment fabric_detailed_summary_weight_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_width_comment fabric_detailed_summary_width_comment { get; set; }

        /// <summary>
        /// add 20230210
        /// </summary>
        public List<Fabric_detailed_summary_over_less_presented_qty> fabric_detailed_summary_over_less_presented_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_detailed_summary_presented_qty> fabric_detailed_summary_presented_qty { get; set; }
    }

    public class Fabric_detailed_summary
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_detailed_summary_fields fabric_detailed_summary_fields { get; set; }
    }

    public class Fabric_defects_classification_color_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_approved_lab_dip
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_continues
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_dying_mill
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_dyeing_mill
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_length_unit
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_order_sheet
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_packing_factory
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_per_lot
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_piece_dyed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_print
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_speed_fabric_machine
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_warehouse
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_yarn_dyed
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_defects_classification_color_acceptance_criteria
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_color_no_of_points_100_sqy
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_copy_original_to_actual
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_width
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_bulk_fabric_swatch
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Fabric_defects_classification_defect_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_location
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_point
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_defect
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_location fabric_defects_classification_detail_fabric_inspection_nested_location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_point fabric_defects_classification_detail_fabric_inspection_nested_point { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_defect fabric_defects_classification_detail_fabric_inspection_nested_defect { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_lines_split_product
    {
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_detail_fabric_inspection_nested_lines_product> fabric_defects_classification_detail_fabric_inspection_nested_lines_product { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_detail_fabric_inspection_nested_lines_split_product> fabric_defects_classification_detail_fabric_inspection_nested_lines_split_product { get; set; }
    }

    public class Fabric_defects_classification_dye_lot_list_dye_lot_title
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_dye_lot_list_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_dye_lot_list_dye_lot_title fabric_defects_classification_dye_lot_list_dye_lot_title { get; set; }
    }

    public class Fabric_defects_classification_dye_lot_list_lines_split_product
    {
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_dye_lot_list_lines_product> fabric_defects_classification_dye_lot_list_lines_product { get; set; }
    }

    public class Fabric_defects_classification_dye_lot_list
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_dye_lot_list_lines_split_product> fabric_defects_classification_dye_lot_list_lines_split_product { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_acceptance_criteria
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_length_original
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_no_of_defect
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_no_of_points
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_roll_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_weight_actual
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_weight_original
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_width_original
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_dye_lot
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_dye_lot_default
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_length_actual
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_no_of_points_100_sqy
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_production_sample_cutted
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }
    public class Fabric_defects_classification_detail_fabric_inspection_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_width_actual
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }


    public class Fabric_defects_classification_detail_fabric_inspection_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_sublines_location
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_sublines_point
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_sublines_sublines
    {
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_sublines_location fabric_defects_classification_detail_fabric_inspection_nested_sublines_location { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_sublines_point fabric_defects_classification_detail_fabric_inspection_nested_sublines_point { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect_description fabric_defects_classification_detail_fabric_inspection_nested_sublines_defect_description { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_nested_sublines
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_detail_fabric_inspection_nested_sublines_sublines> fabric_defects_classification_detail_fabric_inspection_nested_sublines_sublines { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_acceptance_criteria fabric_defects_classification_detail_fabric_inspection_acceptance_criteria { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_length_original fabric_defects_classification_detail_fabric_inspection_length_original { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_no_of_defect fabric_defects_classification_detail_fabric_inspection_no_of_defect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_no_of_points fabric_defects_classification_detail_fabric_inspection_no_of_points { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_roll_number fabric_defects_classification_detail_fabric_inspection_roll_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_weight_actual fabric_defects_classification_detail_fabric_inspection_weight_actual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_weight_original fabric_defects_classification_detail_fabric_inspection_weight_original { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_width_original fabric_defects_classification_detail_fabric_inspection_width_original { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_comment fabric_defects_classification_detail_fabric_inspection_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_dye_lot fabric_defects_classification_detail_fabric_inspection_dye_lot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_dye_lot_default fabric_defects_classification_detail_fabric_inspection_dye_lot_default { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_length_actual fabric_defects_classification_detail_fabric_inspection_length_actual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_no_of_points_100_sqy fabric_defects_classification_detail_fabric_inspection_no_of_points_100_sqy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_production_sample_cutted fabric_defects_classification_detail_fabric_inspection_production_sample_cutted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_result fabric_defects_classification_detail_fabric_inspection_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_width_actual fabric_defects_classification_detail_fabric_inspection_width_actual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_photos fabric_defects_classification_detail_fabric_inspection_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested_sublines fabric_defects_classification_detail_fabric_inspection_nested_sublines { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection_lines_split_product
    {
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_detail_fabric_inspection_lines_product> fabric_defects_classification_detail_fabric_inspection_lines_product { get; set; }
    }

    public class Fabric_defects_classification_detail_fabric_inspection
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_detail_fabric_inspection_lines_split_product> fabric_defects_classification_detail_fabric_inspection_lines_split_product { get; set; }
    }

    public class Fabric_defects_classification_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_color_result> fabric_defects_classification_color_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_approved_lab_dip fabric_defects_classification_approved_lab_dip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_continues fabric_defects_classification_continues { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_dying_mill fabric_defects_classification_dying_mill { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_dyeing_mill fabric_defects_classification_dyeing_mill { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_length_unit fabric_defects_classification_length_unit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_order_sheet fabric_defects_classification_order_sheet { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_overall_result fabric_defects_classification_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_packing_factory fabric_defects_classification_packing_factory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_per_lot fabric_defects_classification_per_lot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_piece_dyed fabric_defects_classification_piece_dyed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_print fabric_defects_classification_print { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_speed_fabric_machine fabric_defects_classification_speed_fabric_machine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_warehouse fabric_defects_classification_warehouse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_yarn_dyed fabric_defects_classification_yarn_dyed { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_color_acceptance_criteria> fabric_defects_classification_color_acceptance_criteria { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_color_no_of_points_100_sqy> fabric_defects_classification_color_no_of_points_100_sqy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_copy_original_to_actual> fabric_defects_classification_copy_original_to_actual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_width> fabric_defects_classification_width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_bulk_fabric_swatch fabric_defects_classification_bulk_fabric_swatch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_defects_classification_defect_photos> fabric_defects_classification_defect_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection_nested fabric_defects_classification_detail_fabric_inspection_nested { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_dye_lot_list fabric_defects_classification_dye_lot_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_detail_fabric_inspection fabric_defects_classification_detail_fabric_inspection { get; set; }
    }

    public class Fabric_defects_classification
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_defects_classification_fields fabric_defects_classification_fields { get; set; }
    }

    public class Fabric_rubbing_test_dry_rubbing_test_customer_requirement
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_product_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_products_init
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_wet_rubbing_test_customer_requirement
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_equipment_used
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_rubbing_test_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_dry_rub
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_dry_rub_over
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_dye_lot
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_dye_lot_default
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_roll
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_rub_test_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_wet_rub
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_wet_rub_over
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }


    public class Fabric_rubbing_test_rubbing_test_lines_photos_wet_rub
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }


    public class Fabric_rubbing_test_rubbing_test_lines_photos_dry_rub
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_lines_product
    {
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_dry_rub fabric_rubbing_test_rubbing_test_lines_dry_rub { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_dry_rub_over fabric_rubbing_test_rubbing_test_lines_dry_rub_over { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_dye_lot fabric_rubbing_test_rubbing_test_lines_dye_lot { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_dye_lot_default fabric_rubbing_test_rubbing_test_lines_dye_lot_default { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_roll fabric_rubbing_test_rubbing_test_lines_roll { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_rub_test_result fabric_rubbing_test_rubbing_test_lines_rub_test_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_wet_rub fabric_rubbing_test_rubbing_test_lines_wet_rub { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_wet_rub_over fabric_rubbing_test_rubbing_test_lines_wet_rub_over { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_photos_wet_rub fabric_rubbing_test_rubbing_test_lines_photos_wet_rub { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines_photos_dry_rub fabric_rubbing_test_rubbing_test_lines_photos_dry_rub { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines_lines_split_product
    {
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_rubbing_test_rubbing_test_lines_lines_product> fabric_rubbing_test_rubbing_test_lines_lines_product { get; set; }
    }

    public class Fabric_rubbing_test_rubbing_test_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_rubbing_test_rubbing_test_lines_lines_split_product> fabric_rubbing_test_rubbing_test_lines_lines_split_product { get; set; }
    }

    public class Fabric_rubbing_test_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_rubbing_test_dry_rubbing_test_customer_requirement> fabric_rubbing_test_dry_rubbing_test_customer_requirement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_rubbing_test_product_result> fabric_rubbing_test_product_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_rubbing_test_products_init> fabric_rubbing_test_products_init { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Fabric_rubbing_test_wet_rubbing_test_customer_requirement> fabric_rubbing_test_wet_rubbing_test_customer_requirement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_equipment_used fabric_rubbing_test_equipment_used { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_overall_result fabric_rubbing_test_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_rubbing_test_lines fabric_rubbing_test_rubbing_test_lines { get; set; }
        /// <summary>
        /// add 20230210
        /// </summary>
        public List<Fabric_rubbing_test_comments> fabric_rubbing_test_comments { get; set; }
    }

    public class Fabric_rubbing_test
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Fabric_rubbing_test_fields fabric_rubbing_test_fields { get; set; }
    }

    public class Additional_photos_fields
    {
    }



    public class Pictures_fabric_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Pictures_fabric_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Pictures_fabric_photos pictures_fabric_photos { get; set; }
    }

    public class Pictures_fabric
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Pictures_fabric_fields pictures_fabric_fields { get; set; }
    }


    public class Pictures_defects_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Pictures_defects_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Pictures_defects_photos pictures_defects_photos { get; set; }
    }

    public class Pictures_defects
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Pictures_defects_fields pictures_defects_fields { get; set; }
    }

    public class Additional_photos_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Pictures_fabric pictures_fabric { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Pictures_defects pictures_defects { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_general_inspection_photos studio_asia_general_inspection_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_document_problem_photos studio_asia_document_problem_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_appendix_photos studio_asia_appendix_photos { get; set; }
    }

    public class Additional_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Additional_photos_fields additional_photos_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Additional_photos_subSections additional_photos_subSections { get; set; }
    }


    //add 20230210

    public class Result_and_conclusion_produced_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Result_and_conclusion_color_conformity_in_uv_light_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_color_conformity_in_uv_light_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Result_and_conclusion_odor_smell_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_odor_smell_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_style_reference
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Result_and_conclusion_total_produced_qty_details
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }



    public class Result_and_conclusion_rejected_4_pt_system_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_rejected_4_pt_system_qty_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_rejected_other_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Result_and_conclusion_rejected_other_qty_rolls
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Fabric_detailed_summary_over_less_presented_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Fabric_detailed_summary_presented_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }
    public class Fabric_defects_classification_detail_fabric_inspection_nested_defect_description
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }


    public class Fabric_rubbing_test_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    #endregion

    #region Studio
    public class Frontpage_studio_asia_cartons_opened
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Frontpage_studio_asia_cartons_presented
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Frontpage_studio_asia_packed_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }

    public class Frontpage_studio_asia_packed_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? report_product_id { get; set; }
    }
    public class Frontpage_studio_asia_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Frontpage_studio_asia_measurement
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_document
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_factory_representative
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_final_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_inspector_firstname
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_inspector_lastname
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_inspector_name
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_packing
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_quality
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_requirement_first
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_requirement_fourth
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_requirement_second
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_requirement_third
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_sa_merchandiser
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_total_cartons_presented
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_total_packed_qty
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_total_packed_qty_percent
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Frontpage_studio_asia_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Frontpage_studio_asia_cartons_opened> frontpage_studio_asia_cartons_opened { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Frontpage_studio_asia_cartons_presented> frontpage_studio_asia_cartons_presented { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Frontpage_studio_asia_packed_qty> frontpage_studio_asia_packed_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Frontpage_studio_asia_packed_qty_percent> frontpage_studio_asia_packed_qty_percent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_comments frontpage_studio_asia_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_comments frontpage_studio_asia_measurement { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_document frontpage_studio_asia_document { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_factory_representative frontpage_studio_asia_factory_representative { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_final_result frontpage_studio_asia_final_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_inspector_firstname frontpage_studio_asia_inspector_firstname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_inspector_lastname frontpage_studio_asia_inspector_lastname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_inspector_name frontpage_studio_asia_inspector_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_packing frontpage_studio_asia_packing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_quality frontpage_studio_asia_quality { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_requirement_first frontpage_studio_asia_requirement_first { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_requirement_fourth frontpage_studio_asia_requirement_fourth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_requirement_second frontpage_studio_asia_requirement_second { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_requirement_third frontpage_studio_asia_requirement_third { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_sa_merchandiser frontpage_studio_asia_sa_merchandiser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_total_cartons_presented frontpage_studio_asia_total_cartons_presented { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_total_packed_qty frontpage_studio_asia_total_packed_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_total_packed_qty_percent frontpage_studio_asia_total_packed_qty_percent { get; set; }
    }

    public class Frontpage_studio_asia
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Frontpage_studio_asia_fields frontpage_studio_asia_fields { get; set; }
    }




    public class Studio_asia_workmanship_packing_dynamic_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Studio_asia_workmanship_packing_dynamic_lines_lines> studio_asia_workmanship_packing_dynamic_lines_lines { get; set; }
    }
    public class Studio_asia_workmanship_packing_dynamic_lines_comment
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_packing_dynamic_lines_nb_defects
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_packing_dynamic_lines_packing_defect_name
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_packing_dynamic_lines_packing_defect_code
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }


    public class Studio_asia_workmanship_packing_dynamic_lines_packing_defect_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Studio_asia_workmanship_packing_dynamic_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_dynamic_lines_comment studio_asia_workmanship_packing_dynamic_lines_comment { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_dynamic_lines_nb_defects studio_asia_workmanship_packing_dynamic_lines_nb_defects { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_dynamic_lines_packing_defect_name studio_asia_workmanship_packing_dynamic_lines_packing_defect_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_dynamic_lines_packing_defect_code studio_asia_workmanship_packing_dynamic_lines_packing_defect_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_dynamic_lines_packing_defect_photos studio_asia_workmanship_packing_dynamic_lines_packing_defect_photos { get; set; }
    }
    public class Studio_asia_workmanship_packing_packing_defect_total
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_packing_packing_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_packing_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_dynamic_lines studio_asia_workmanship_packing_dynamic_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_packing_defect_total studio_asia_workmanship_packing_packing_defect_total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_packing_result studio_asia_workmanship_packing_packing_result { get; set; }
    }

    public class Studio_asia_workmanship_packing
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_packing_fields studio_asia_workmanship_packing_fields { get; set; }
    }

    public class Studio_asia_workmanship_report_status_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_report_status_product_test_report_status_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_report_status_report_validity_date
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_report_status_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status_comments studio_asia_workmanship_report_status_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status_report_validity_date studio_asia_workmanship_report_status_report_validity_date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status_yes studio_asia_workmanship_report_status_yes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status_no studio_asia_workmanship_report_status_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status_not_applicable studio_asia_workmanship_report_status_not_applicable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status_product_test_report_status_result studio_asia_workmanship_report_status_product_test_report_status_result { get; set; }
    }

    public class Studio_asia_workmanship_report_status
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_workmanship_report_status_fields studio_asia_workmanship_report_status_fields { get; set; }
    }

    public class Onsite_tests_c_f_kids_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Onsite_tests_onsite_testing_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Onsite_tests_overall_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Onsite_tests_overview_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Onsite_tests_packing_verification_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Onsite_tests_size_weight_measurements_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Onsite_tests_special_testing_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Onsite_tests_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_c_f_kids_result onsite_tests_c_f_kids_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_onsite_testing_result onsite_tests_onsite_testing_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_overall_result onsite_tests_overall_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_overview_result onsite_tests_overview_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_packing_verification_result onsite_tests_packing_verification_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_size_weight_measurements_result onsite_tests_size_weight_measurements_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_special_testing_result onsite_tests_special_testing_result { get; set; }
    }

    public class Studio_asia_onsite_test_barcode
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_barcode_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_barcode_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_c_f_kids_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_item
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_line_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_serial_no
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_item_name
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_c_f_kids_dynamic_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_item studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_line_result studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_line_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_sample_size studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_serial_no studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_serial_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_item_name studio_asia_onsite_test_c_f_kids_dynamic_lines_c_f_kids_item_name { get; set; }
    }

    public class Studio_asia_onsite_test_c_f_kids_dynamic_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Studio_asia_onsite_test_c_f_kids_dynamic_lines_lines> studio_asia_onsite_test_c_f_kids_dynamic_lines_lines { get; set; }
    }

    public class Studio_asia_onsite_test_carton_dimension_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_dimension_size_measured_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_dimension_size_sample_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_dimension_size_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_dimension_size_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_weight
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_weight_measured_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_weight_sample_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_weight_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_carton_weight_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_instruction_manual
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_instruction_manual_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_instruction_manual_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_labeling
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_labeling_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_labeling_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_onsite_testing_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_item
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_line_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_serial_no
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    public class Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_item_name
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_onsite_testing_dynamic_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_item studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_line_result studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_line_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_sample_size studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_serial_no studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_serial_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_item_name studio_asia_onsite_test_onsite_testing_dynamic_lines_onsite_testing_item_name { get; set; }
    }

    public class Studio_asia_onsite_test_onsite_testing_dynamic_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Studio_asia_onsite_test_onsite_testing_dynamic_lines_lines> studio_asia_onsite_test_onsite_testing_dynamic_lines_lines { get; set; }
    }

    public class Studio_asia_onsite_test_packing_verification_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_size_measured_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_size_sample_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_size_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_size_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_weight
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_weight_measured_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_weight_sample_data
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_weight_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_product_weight_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_shipping_mark_ean
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_shipping_mark_ean_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_shipping_mark_ean_selected_carton_number
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_size_weight_measurements_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_comments
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_inspection_item
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_line_result
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_method_criterion
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_no
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_sample_size
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_dynamic_lines_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_inspection_item studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_inspection_item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_line_result studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_line_result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_method_criterion studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_method_criterion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_no studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_sample_size studio_asia_onsite_test_special_testing_dynamic_lines_special_testing_sample_size { get; set; }
    }

    public class Studio_asia_onsite_test_special_testing_dynamic_lines
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Studio_asia_onsite_test_special_testing_dynamic_lines_lines> studio_asia_onsite_test_special_testing_dynamic_lines_lines { get; set; }
    }


    public class Studio_asia_onsite_test_size_weight_measurements_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }



    public class Studio_asia_onsite_test_onsite_testing_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }



    public class Studio_asia_onsite_test_c_f_kids_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }



    public class Studio_asia_onsite_test_special_testing_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }



    public class Studio_asia_onsite_test_packing_verification_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Studio_asia_onsite_test_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_barcode studio_asia_onsite_test_barcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_barcode_sample_size studio_asia_onsite_test_barcode_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_barcode_selected_carton_number studio_asia_onsite_test_barcode_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_comments studio_asia_onsite_test_c_f_kids_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_dynamic_lines studio_asia_onsite_test_c_f_kids_dynamic_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_dimension_size studio_asia_onsite_test_carton_dimension_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_dimension_size_measured_data studio_asia_onsite_test_carton_dimension_size_measured_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_dimension_size_sample_data studio_asia_onsite_test_carton_dimension_size_sample_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_dimension_size_sample_size studio_asia_onsite_test_carton_dimension_size_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_dimension_size_selected_carton_number studio_asia_onsite_test_carton_dimension_size_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_weight studio_asia_onsite_test_carton_weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_weight_measured_data studio_asia_onsite_test_carton_weight_measured_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_weight_sample_data studio_asia_onsite_test_carton_weight_sample_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_weight_sample_size studio_asia_onsite_test_carton_weight_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_carton_weight_selected_carton_number studio_asia_onsite_test_carton_weight_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_instruction_manual studio_asia_onsite_test_instruction_manual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_instruction_manual_sample_size studio_asia_onsite_test_instruction_manual_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_instruction_manual_selected_carton_number studio_asia_onsite_test_instruction_manual_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_labeling studio_asia_onsite_test_labeling { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_labeling_sample_size studio_asia_onsite_test_labeling_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_labeling_selected_carton_number studio_asia_onsite_test_labeling_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_comments studio_asia_onsite_test_onsite_testing_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_dynamic_lines studio_asia_onsite_test_onsite_testing_dynamic_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_packing_verification_comments studio_asia_onsite_test_packing_verification_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_size studio_asia_onsite_test_product_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_size_measured_data studio_asia_onsite_test_product_size_measured_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_size_sample_data studio_asia_onsite_test_product_size_sample_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_size_sample_size studio_asia_onsite_test_product_size_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_size_selected_carton_number studio_asia_onsite_test_product_size_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_weight studio_asia_onsite_test_product_weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_weight_measured_data studio_asia_onsite_test_product_weight_measured_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_weight_sample_data studio_asia_onsite_test_product_weight_sample_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_weight_sample_size studio_asia_onsite_test_product_weight_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_product_weight_selected_carton_number studio_asia_onsite_test_product_weight_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_shipping_mark_ean studio_asia_onsite_test_shipping_mark_ean { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_shipping_mark_ean_sample_size studio_asia_onsite_test_shipping_mark_ean_sample_size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_shipping_mark_ean_selected_carton_number studio_asia_onsite_test_shipping_mark_ean_selected_carton_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_size_weight_measurements_comments studio_asia_onsite_test_size_weight_measurements_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_comments studio_asia_onsite_test_special_testing_comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_dynamic_lines studio_asia_onsite_test_special_testing_dynamic_lines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_size_weight_measurements_photos studio_asia_onsite_test_size_weight_measurements_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_onsite_testing_photos studio_asia_onsite_test_onsite_testing_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_c_f_kids_photos studio_asia_onsite_test_c_f_kids_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_special_testing_photos studio_asia_onsite_test_special_testing_photos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_packing_verification_photos studio_asia_onsite_test_packing_verification_photos { get; set; }
    }

    public class Studio_asia_onsite_test
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test_fields studio_asia_onsite_test_fields { get; set; }
    }

    public class Onsite_tests_subSections
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_onsite_test studio_asia_onsite_test { get; set; }
    }

    public class Onsite_tests
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_fields onsite_tests_fields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Onsite_tests_subSections onsite_tests_subSections { get; set; }
    }





    public class Studio_asia_general_inspection_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Studio_asia_general_inspection_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_general_inspection_photos_photos studio_asia_general_inspection_photos_photos { get; set; }
    }

    public class Studio_asia_general_inspection_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_general_inspection_photos_fields studio_asia_general_inspection_photos_fields { get; set; }
    }


    public class Studio_asia_document_problem_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Studio_asia_document_problem_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_document_problem_photos_photos studio_asia_document_problem_photos_photos { get; set; }
    }

    public class Studio_asia_document_problem_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_document_problem_photos_fields studio_asia_document_problem_photos_fields { get; set; }
    }



    public class Studio_asia_appendix_photos_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Files> files { get; set; }
    }

    public class Studio_asia_appendix_photos_fields
    {
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_appendix_photos_photos studio_asia_appendix_photos_photos { get; set; }
    }

    public class Studio_asia_appendix_photos
    {
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ordering { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string reference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string referenceNonUnique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Studio_asia_appendix_photos_fields studio_asia_appendix_photos_fields { get; set; }
    }


    public class Studio_asia_workmanship_report_status_yes
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_report_status_no
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }

    public class Studio_asia_workmanship_report_status_not_applicable
    {
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
    }
    #endregion

    #endregion

    #endregion 

}
