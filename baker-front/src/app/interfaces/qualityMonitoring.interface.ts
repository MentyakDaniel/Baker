export interface QualityMonitoring {
    id: number,
    bunSaleId: string,
    nextPrice?: number,
    toNextPrice: string,
    isThrow: boolean,
    timeStamp: Date
}