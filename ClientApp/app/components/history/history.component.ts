import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";
import { Order, OrderItem } from "../shared/order";

@Component({
	selector: "history",
	templateUrl: "history.component.html",
	styleUrls: ['./history.component.css', '../app/app.component.css']
})
export class HistoryComponent implements OnInit{

	errorMessage: string;

	orderHistory: Order[];

	constructor(private data: DataService, private router: Router) {

	}

	ngOnInit(): void {
		if (this.data.loginRequired) {
			this.router.navigate(["login"]);
		}
		this.data.getOrderHistory(this.data.userName).subscribe(success => {
			if (success) {
				this.orderHistory = this.data.orderHistory;
			}
		})
	}

	
}