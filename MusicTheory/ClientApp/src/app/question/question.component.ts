import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuestionService } from './question.service';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.css']
})
export class QuestionComponent implements OnInit {
  lesson: Lesson;
  currentQuestionIndex = 0;
  selectedOptionId: number;
  displayResultPanel = false;

  get currentQuestion() {
    return this.lesson && this.lesson.questions[this.currentQuestionIndex];
  }

  get isAnswerCorrect() {
    return this.currentQuestion.answerId === this.selectedOptionId;
  }

  get correctAnswer() {
    return this.currentQuestion.textOptions.filter(x => x.id === this.currentQuestion.answerId)[0];
  }

  constructor(private route: ActivatedRoute,
    private service: QuestionService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const id = params['lessonId'];
      this.service.getLesson(id).subscribe(lesson => {
        this.lesson = lesson;
      });
    });
  }

  checkIfCorrect() {
    this.displayResultPanel = true;
    this.currentQuestion.answeredCorrectly = this.isAnswerCorrect;
  }

  setSelectedOption(optionId: number) {
    this.selectedOptionId = optionId;
  }

  moveToNext() {
    this.selectedOptionId = null;
    this.displayResultPanel = false;
    if (this.currentQuestionIndex <= this.lesson.questions.length - 2) {
      this.currentQuestionIndex++;
    } else {
      this.endLesson();
    }
  }

  skip() {
    this.currentQuestion.answeredCorrectly = false;
    this.moveToNext();
  }

  endLesson() {
    console.log('finish lesson', this.lesson.questions);
  }
}
