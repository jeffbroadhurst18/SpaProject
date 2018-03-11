import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";
import { Order, OrderItem } from "../shared/order";
import { TransformStatusPipe } from "./process.component.pipe"
import { Location } from '@angular/common';

@Component({
	selector: "process",
	templateUrl: "process.component.html",
	styleUrls: ['./process.component.css', '../app/app.component.css']
})
export class ProcessComponent implements OnInit{

	errorMessage: string;

	allOrders: Order[];
	detailOrderItems: OrderItem[];
	selectedOrderId: number;
	sortOrder: string;
	
	constructor(private data: DataService, private router: Router,
				private location: Location) {
		this.sortOrder = "totalAsc";
	}

	ngOnInit(): void {
		if (this.data.loginRequired) {
			this.router.navigate(["login"]);
		}
		
		this.data.getAllOrders().subscribe(success => {
			if (success) {

				this.allOrders = this.returnSortedOrders(this.sortOrder);

				if (this.allOrders.length > 0) {
					this.setSelected(this.allOrders[0].orderId);
				}
			}
		})
	}

	returnSortedOrders(sortOrder: string): Order[] 
		{
		switch (sortOrder) {
			case "totalAsc":
				return this.data.allOrders.sort((n1, n2) => n1.orderTotal - n2.orderTotal);
			case "totalDesc":
				return this.data.allOrders.sort((n1, n2) => n2.orderTotal - n1.orderTotal);
			default:
				return this.data.allOrders.sort((n1, n2) => n1.orderTotal - n2.orderTotal);
		}
	}


	showDetail(orderId: number) {
		this.data.getOrderItems(orderId).subscribe(success => {
			if (success) {
				this.detailOrderItems = this.data.orderItems;
			}
		})
	}

	setSelected(orderId:number) {
		this.selectedOrderId = orderId;
		this.showDetail(orderId);
	}

	setStatus(selectedOrder: Order, newStatus: number) {
		this.data.setStatus(selectedOrder, newStatus).subscribe(success => {
			if (success) {
				let amendedOrder = this.allOrders.find(a => a.orderId == selectedOrder.orderId);
				if (amendedOrder) {
					amendedOrder.orderStatus = newStatus;
				}
			}
		})
	}

	sortByTotal() {
		switch (this.sortOrder) {
			case "totalAsc":
				this.sortOrder = "totalDesc";
				break;
			case "totalDesc":
				this.sortOrder = "totalAsc";
				break;
			default:
				this.sortOrder = "totalAsc";
		}


		this.allOrders = this.returnSortedOrders(this.sortOrder);
	}
}