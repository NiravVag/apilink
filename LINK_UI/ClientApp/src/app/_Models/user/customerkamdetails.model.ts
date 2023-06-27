import { ITSupporEmail, LBLServiceDesk } from "src/app/components/common/static-data-common";

export class CustomerKAMDetail {
    name: string;
    email: string;
    phoneNumber: string;
    constructor() {
        this.name = LBLServiceDesk;
        this.email = ITSupporEmail;
    }
}