import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'jsonPrettyPrint',
})
export class JsonPrettyPrintPipe implements PipeTransform {
  transform(value: string): string {
    try {
      const parsedValue = JSON.parse(value);
      return JSON.stringify(parsedValue, null, 2);
    } catch (e) {
      return value;
    }
  }
}
