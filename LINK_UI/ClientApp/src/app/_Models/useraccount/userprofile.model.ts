
export class UserProfileModel {
    public userId: number;
    public userName: string;
    public displayName: string;
    public emailId: string;
    public phone: string;
    public profileImageName: string;
    public profileImageUrl: string;
}

export enum ResponseResult {
    Success = 1,
    Faliure = 2,
    RequestNotCorrectFormat = 3,
    Error = 4,
    NotFound = 5,
    EmailAlreadyExists = 6
}

export class SaveUserProfileResponse {
    result: ResponseResult;
    id: number;
}

export class GetUserProfileResponse {
    result: ResponseResult;
    data: UserProfileModel;
}