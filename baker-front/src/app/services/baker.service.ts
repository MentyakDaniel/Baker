import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";
import { BunSale } from "../interfaces/bunSale.interface";
import { environment } from "src/environments/environments";

@Injectable()
export class BakerService
{
    constructor(private http: HttpClient)
    {}

    getAllSales(): Observable<BunSale[]> {
        return this.http.get<BunSale[]>(`${environment.apiUrl}/baker`);
    }

    bake(count:number): Observable<boolean> {
        return this.http.post<boolean>(`${environment.apiUrl}/baker/bake`, count,{
            headers: new HttpHeaders({ 'accept': '*/*', 'Content-Type': 'application/json' })
        });
    }
}