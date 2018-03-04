import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'transformStatus' })
export class TransformStatusPipe implements PipeTransform {
	transform(input: number): string {
		switch (input) {
			case 0: { return 'Pending'; }
			case 1: { return 'Completed'; }
			case 2: { return 'Cancelled'; }
			case 3: { return 'Returned'; }
			default: { return 'Pending'; }
		}
	}
}