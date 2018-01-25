import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";

@Component({
	selector: "checkout",
	templateUrl: "checkout.component.html",
	styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent {
	errorMessage: string;

	constructor(private data: DataService, private router: Router) {

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
}