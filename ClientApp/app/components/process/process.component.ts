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
	
	selectedOption: string;
	
	constructor(private data: DataService, private router: Router,
				private location: Location) {
		this.sortOrder = "totalAsc";
	}

	ngOnInit(): void {
		if (this.data.loginRequired) {
			this.router.navigate(["login"]);
		}

		let options = [
			{ name: "option1", value: 1 },
			{ name: "option2", value: 2 }
		]
		this.selectedOption = options[0].name;

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
			case "dateAsc":
				return this.data.allOrders.sort((n1, n2) => Date.parse(n1.orderDate.toString()) - Date.parse(n2.orderDate.toString());
			case "dateDesc":
				return this.data.allOrders.sort((n1, n2) => Date.parse(n2.orderDate.toString()) - Date.parse(n1.orderDate.toString());
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

	sortByDate() {
		switch (this.sortOrder) {
			case "dateAsc":
				this.sortOrder = "dateDesc";
				break;
			case "dateDesc":
				this.sortOrder = "dateAsc";
				break;
			default:
				this.sortOrder = "dateAsc";
		}
		this.allOrders = this.returnSortedOrders(this.sortOrder);
	}
}