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
	displayedOrders: Order[];
	detailOrderItems: OrderItem[];
	selectedOrderId: number;
	sortOrder: string;
	options: string[];
	selectedUser: string;
	selectedStatus: number;
	selectedOption: number;
	searchCriteria: Object;
	public criterias = [
		{ value: '1', display: 'User' },
		{ value: '2', display: 'Status' }
	];
	
	constructor(private data: DataService, private router: Router,
				private location: Location) {
		this.sortOrder = "totalAsc";
	}

	ngOnInit(): void {
		if (this.data.loginRequired) {
			this.router.navigate(["login"]);
		}

		this.data.getUsers().subscribe(success => {
			if (success) {
				this.options = this.data.allUsers;
			}
		});

		this.data.getAllOrders().subscribe(success => {
			if (success) {

				this.allOrders = this.returnSortedOrders(this.sortOrder);
				this.displayedOrders = this.allOrders;

				if (this.displayedOrders.length > 0) {
					this.setSelected(this.displayedOrders[0].orderId);
				}
			}
		})
	}

	handleChange() {
		if (this.searchCriteria == '1') {
			this.options = this.data.allUsers;
			this.displayedOrders = this.allOrders;
		}
		else {
			this.options = ['Pending', 'Completed', 'Cancelled', 'Returned'];
			this.displayedOrders = this.allOrders;
		}
	}

	filterUser(selectedUser: string) {
		if (this.searchCriteria == '1') {
			this.selectedUser = selectedUser;
			this.displayedOrders = this.allOrders.filter((order) => order.userName == this.selectedUser);
		}
		else {
			switch (selectedUser) {
				case 'Pending':
					this.selectedStatus = 0;
					break;
				case 'Completed':
					this.selectedStatus = 1;
					break;
				case 'Cancelled':
					this.selectedStatus = 2;
					break;
				case 'Returned':
					this.selectedStatus = 3;
					break;
				default:
					this.selectedStatus = 0;
			}

			this.displayedOrders = this.allOrders.filter((order) => order.orderStatus == this.selectedStatus);
		}
	}

	returnSortedOrders(sortOrder: string): Order[] 
		{
		switch (sortOrder) {
			case "totalAsc":
				return this.data.allOrders.sort((n1, n2) => n1.orderTotal - n2.orderTotal);
			case "totalDesc":
				return this.data.allOrders.sort((n1, n2) => n2.orderTotal - n1.orderTotal);
			case "dateAsc":
				return this.data.allOrders.sort((n1, n2) => Date.parse(n1.orderDate.toString()) - Date.parse(n2.orderDate.toString()));
			case "dateDesc":
				return this.data.allOrders.sort((n1, n2) => Date.parse(n2.orderDate.toString()) - Date.parse(n1.orderDate.toString()));
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
				let amendedOrder = this.displayedOrders.find(a => a.orderId == selectedOrder.orderId);
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
		if (this.searchCriteria == '1') {
			this.displayedOrders = this.returnSortedOrders(this.sortOrder).filter((order) => order.userName == this.selectedUser);
		}
		else {
			this.displayedOrders = this.returnSortedOrders(this.sortOrder).filter((order) => order.orderStatus == this.selectedStatus);
		}
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

		if (this.searchCriteria == '1') {
			this.displayedOrders = this.returnSortedOrders(this.sortOrder).filter((order) => order.userName == this.selectedUser);
		}
		else {
			this.displayedOrders = this.returnSortedOrders(this.sortOrder).filter((order) => order.orderStatus == this.selectedStatus);
		}
	}
}