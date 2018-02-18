import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'transformStatus' })
export class TransformStatusPipe implements PipeTransform {
	transform(input: number): string {
		switch (input) {
			case 0: { return 'Pending'; }
			case 1: { return 'Fulfilled'; }
			case 2: { return 'Completed'; }
			default: { return 'Pending'; }
		}
	}
}