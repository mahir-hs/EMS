import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JsonPrettyPrintPipe } from '../json-pretty-print.pipe';

@NgModule({
  declarations: [JsonPrettyPrintPipe],
  exports: [JsonPrettyPrintPipe],
})
export class JsonPrettyPrintModule {}
