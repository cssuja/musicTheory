import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddLessonComponent } from '../add-lesson/add-lesson.component';
import { FormsModule } from '@angular/forms';
import { AddLessonService } from './add-lesson.service';

@NgModule({
  declarations: [AddLessonComponent],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    AddLessonComponent
  ],
  providers: [AddLessonService]
})
export class AddLessonModule { }
