import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-lesson',
  templateUrl: './add-lesson.component.html',
  styleUrls: ['./add-lesson.component.css']
})
export class AddLessonComponent implements OnInit {
  lesson: Lesson = {} as Lesson;
  currentQuestion: QuestionModel;
  currentTextOption: TextQuestionOption;
  constructor() { }

  ngOnInit() {
    this.lesson.questions = [];
    this.initialiseCurrentQuestion();
    this.initialiseCurrentTextOption();
  }

  initialiseCurrentQuestion() {
    this.currentQuestion = {} as QuestionModel;
    this.currentQuestion.textOptions = [];
  }

  initialiseCurrentTextOption() {
    this.currentTextOption = {} as TextQuestionOption;
  }

  addQuestion() {
    this.lesson.questions = [...this.lesson.questions, this.currentQuestion];
    this.initialiseCurrentQuestion();
  }
  addTextOption() {
    this.currentQuestion.textOptions = [...this.currentQuestion.textOptions, this.currentTextOption];
    this.initialiseCurrentTextOption();
  }

  submit() {
    console.log(this.lesson);
  }

}
