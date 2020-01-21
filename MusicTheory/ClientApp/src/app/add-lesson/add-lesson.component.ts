import { Component, OnInit } from '@angular/core';
import { AddLessonService } from './add-lesson.service';

@Component({
  selector: 'app-add-lesson',
  templateUrl: './add-lesson.component.html',
  styleUrls: ['./add-lesson.component.css']
})
export class AddLessonComponent implements OnInit {
  lesson: Lesson = {} as Lesson;
  currentQuestion: QuestionModel;
  currentTextOption: QuestionOption;
  currentOptionIsCorrect = false;
  constructor(private addLessonService: AddLessonService) { }

  ngOnInit() {
    this.initialiseLesson();
  }

  initialiseLesson() {
    this.lesson = {} as Lesson;
    this.lesson.questions = [];
    this.initialiseCurrentQuestion();
    this.initialiseCurrentTextOption();
  }

  initialiseCurrentQuestion() {
    this.currentQuestion = {} as QuestionModel;
    this.currentQuestion.options = [];
    this.currentQuestion.typeId = 1;
  }

  initialiseCurrentTextOption() {
    this.currentTextOption = {} as QuestionOption;
  }

  addQuestion() {
    this.lesson.questions = [...this.lesson.questions, this.currentQuestion];
    this.initialiseCurrentQuestion();
  }
  addTextOption() {
    this.currentQuestion.options = [...this.currentQuestion.options, this.currentTextOption];
    this.initialiseCurrentTextOption();
  }

  submit() {
    console.log('submit');
    this.addLessonService.mergeLesson(this.lesson).subscribe();
    this.initialiseLesson();
    console.log(this.lesson);
  }

}
