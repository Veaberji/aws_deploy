import { TextInputPipe } from './text-input.pipe';

describe('TextInputPipe', () => {
  let pipe: TextInputPipe;

  beforeEach(() => {
    pipe = new TextInputPipe();
  });

  it('should be created', () => {
    expect(pipe).toBeTruthy();
  });

  it('#getAllowedInput should return empty string when empty string is passed', () => {
    const result = pipe.transform('');

    expect(result).toBe('');
  });

  it(`#getAllowedInput should return string
      that contains only lowercase letters, numbers, and spaces when input string is passed`, () => {
    const testInput: string = 'ABC abc!"#$%&\'()*+,-./:;<=>?@[]^_`{|}~';
    const expectedOutput: string = 'abc abc';

    const result = pipe.transform(testInput);

    expect(result).toBe(expectedOutput);
  });
});
