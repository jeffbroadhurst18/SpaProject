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

	constructor(private data: DataService, private router: Router,
				private location: Location) {

	}

	ngOnInit(): void {
		if (this.data.loginRequired) {
			this.router.navigate(["login"]);
		}
		
		this.data.getAllOrders().subscribe(success => {
			if (success) {
				this.allOrders = this.data.allOrders;
				if (this.allOrders.length > 0) {
					this.setSelected(this.allOrders[0].orderId);
				}
			}
		})
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
				for (let order of this.allOrders) {
					if (order.orderId == selectedOrder.orderId) {
						order.orderStatus = newStatus;
			}
		}
	}
}