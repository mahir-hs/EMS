import { JsonPrettyPrintPipe } from './json-pretty-print.pipe';

describe('JsonPrettyPrintPipe', () => {
  it('create an instance', () => {
    const pipe = new JsonPrettyPrintPipe();
    expect(pipe).toBeTruthy();
  });
});
