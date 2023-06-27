export class PushWorkerStatus 
{
    public isSubscribed : boolean;
    public isSupported : boolean; 
    public isInProgress : boolean;

    constructor()
    {
        this.isSubscribed = false ; 
        this.isSupported = false ; 
        this.isInProgress = false ; 

    }
}
