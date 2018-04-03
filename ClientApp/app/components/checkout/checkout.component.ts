import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";

@Component({
	selector: "checkout",
	templateUrl: "checkout.component.html",
	styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit{

	errorMessage: string;

	constructor(private data: DataService, private router: Router) {

	}

	ngOnInit(): void {
		this.data.calculateTotal();
		this.data.getAddress(this.data.userName).subscribe(success => { });
		this.data.getUser(this.data.userName).subscribe(success => { });
	}

	onCheckout() {
		this.data.checkout()
			.subscribe(success => {
				if (success) {
					this.router.navigate([""]);
				}
			}, err => {
				this.errorMessage = "Failed to save order";
			})
	}

	recalculateTotal() {
		console.log('filter change called');
		this.data.calculateTotal();
	}
}