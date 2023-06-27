



export enum TCFStage {
  New = 1,
  ScopeDefinition = 2,
  InProgress = 3,
  ToBeFinalized = 4,
  Completed = 5,
  TerminatedByClient = 6,
  Cancel = 7,
  Pending = 8,
  Expired = 9,
  Waiting = 11
}

export enum TrafficLightColor {
  Red = 1,
  Orange = 2,
  Green = 3
}

export enum TCFDownloadDocument {
  TCFReport = 1,
  AllDocuments = 2,
  AllValidDocuments = 3,
  SpecificDocument = 4
}

export enum TCFTaskType {
  InProgress = 1,
  Completed = 2
}

export enum TCFSummaryPageType {
  FromDashboard = 1
}

export enum TCFStageResponse {
  Success = 1,
  NotFound = 2
}

export enum TCFDepartmentResponse {
  Success = 1,
  NotFound = 2
}

export enum TCFStandardsResponse {
  Success = 1,
  NotFound = 2
}

export enum TCFDocumentTypeResponse {
  Success = 1,
  NotFound = 2
}

export enum TCFDocumentIssuerResponse {
  Success = 1,
  NotFound = 2
}

export enum TCFDownloadDocumentResponse {
  Success = 1,
  NotFound = 2
}

