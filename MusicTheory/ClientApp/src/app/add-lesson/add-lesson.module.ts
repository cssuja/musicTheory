import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddLessonComponent } from '../add-lesson/add-lesson.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [AddLessonComponent],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    AddLessonComponent
  ]
})
export class AddLessonModule { }
