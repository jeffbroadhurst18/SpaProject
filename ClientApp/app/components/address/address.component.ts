﻿import { Component, OnInit } from "@angular/core";
import { DataService } from "../shared/data.service";
import { Router } from "@angular/router";
import { Address } from "../shared/address";
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
	selector: "address",
	templateUrl: "address.component.html",
	styleUrls: ['./address.component.css']
})
export class AddressComponent implements OnInit {

	userName: string;
	address: Address;
	contactForm: FormGroup;
	isNew: boolean;
	
	ngOnInit(): void {
		this.userName = this.data.userName;
		this.address = this.data.getAddress(this.userName);

		//this.address = new Address();
		//this.address.username = this.userName;
		//this.address.addressline1 = "18 Current Street";
		//this.address.addressline2 = "Wertley";
		//this.address.city = "York";
		//this.address.postcode = "YO12 3DF";
		//this.address.country = "UK";
		//this.address.telephone = "01344 454343";
		this.isNew = false;

		//this.address = new Address();
		//this.address.username = this.userName;
		//this.isNew = true;
		this.buildForm();

	}

	errorMessage: string;

	constructor(private data: DataService, private router: Router, private fb: FormBuilder) {
		
	}

	buildForm() {
		this.contactForm = this.fb.group({
			'username': [this.userName, Validators.required],
			'addressline1': ['', Validators.required],
			'addressline2': [''],
			'city': ['', Validators.required],
			'postcode': ['', Validators.required],
			'country': ['', Validators.required],
			'telephone': ['', Validators.required],
		});

		this.contactForm.controls['username'].disable();
	}

	

	submit() {
		if (this.isNew) {
			//		this.data.addAddress(this.address).subscribe((data) => this.afterSave());
		} else {
			//		this.data.updateAddress(this.address).subscribe((data) => this.afterSave());
		}
	}
	afterSave() {
		this.router.navigate(['']);
	}

	cancel() {
		this.router.navigate(['']);
	}

}