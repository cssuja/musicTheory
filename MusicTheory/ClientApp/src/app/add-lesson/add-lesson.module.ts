import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddLessonComponent } from '../add-lesson/add-lesson.component';
import { FormsModule } from '@angular/forms';
import { AddLessonService } from './add-lesson.service';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

@NgModule({
  declarations: [AddLessonComponent],
  imports: [
    CommonModule,
    FormsModule,
    MatSelectModule,
    MatFormFieldModule
  ],
  exports: [
    AddLessonComponent
  ],
  providers: [AddLessonService]
})
export class AddLessonModule { }
