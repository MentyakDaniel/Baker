import { Component, OnInit } from '@angular/core';
import { BakerService } from './services/baker.service';
import { SalesTable } from './interfaces/salesTable.interface';
import { tap } from 'rxjs';
import { BunNameEnum } from './interfaces/bunType.interface';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Пекарня';
  sales!: SalesTable[];
  displayedColumns: string[] = ['name', 'defaultPrice', 'currentPrice', 'nextPrice', 'timeToNext'];
  countBun: number | null = null;

  constructor(private bakerService: BakerService) {

  }

  ngOnInit(): void {
    this.updateSales();

    setInterval(() => {
      this.updateSales();
    }, 30000);
  }


  private updateSales() {
    this.bakerService.getAllSales().pipe(
      tap(result => {
        this.sales = result.map<SalesTable>(x => ({
          name: BunNameEnum[x.bunType.name],
          defaultPrice: x.bunType.defaultPrice,
          currentPrice: x.price,
          nextPrice: x.monitoring.nextPrice,
          timeToNext: x.monitoring.toNextPrice
        }));
      })
    ).subscribe();
  }

  public bakeSales() {
    this.bakerService.bake(this.countBun ?? 1).pipe(
      tap(result => {
        if (result)
          this.updateSales();
      })
    ).subscribe();
  }
}
