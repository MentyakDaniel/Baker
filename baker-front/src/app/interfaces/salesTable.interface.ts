export interface SalesTable {
    name:string,
    defaultPrice:number,
    currentPrice:number,
    nextPrice:number | undefined,
    timeToNext: string
}