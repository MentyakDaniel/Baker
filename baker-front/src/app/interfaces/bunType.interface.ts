export interface BunType {
    id: string,
    name: BunNameEnum,
    defaultPrice: number,
    sellTerm: Date,
    controlTerm: Date
}

export enum BunNameEnum {
    Круассан = 1,
    Крендель,
    Багет,
    Сметанник,
    Батон
}