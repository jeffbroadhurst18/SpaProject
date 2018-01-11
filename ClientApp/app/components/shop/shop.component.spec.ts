/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { assert } from 'chai';
import { ShopComponent } from './shop.component';
import { TestBed, async, ComponentFixture } from '@angular/core/testing';

let fixture: ComponentFixture<ShopComponent>;

describe('Shop component', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({ declarations: [ShopComponent] });
        fixture = TestBed.createComponent(ShopComponent);
        fixture.detectChanges();
    });

    
});
