import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'textInput',
})
export class TextInputPipe implements PipeTransform {
  transform(value: string): string {
    return value ? this.getAllowedInput(value) : '';
  }

  private getAllowedInput(value: string): string {
    let word: string = '';
    for (var i = 0; i < value.length; i++) {
      if (value[i].match('[A-Za-z0-9 ]')) {
        word += value[i];
      }
    }
    return word.toLowerCase();
  }
}
