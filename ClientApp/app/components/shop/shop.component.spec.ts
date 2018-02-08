/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { assert } from 'chai';
import { ShopComponent } from './shop.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';

let fixture: ComponentFixture<ShopComponent>;

describe('Component: ShopComponent', () => {
	let component: ShopComponent;

	beforeEach(() => {
		TestBed.configureTestingModule({ declarations: [ShopComponent] });
		fixture = TestBed.createComponent(ShopComponent);
		fixture.detectChanges();
		component = fixture.componentInstance;
	});

	it('should have a defined compnent', () => {
		expect(component).toBeDefined();
	});

});
