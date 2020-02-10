import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionService } from './question.service';

@Component({
  selector: 'app-question',
  templateUrl: './question.component.html',
  styleUrls: ['./question.component.scss']
})
export class QuestionComponent implements OnInit {
  lesson: Lesson;
  currentQuestionIndex = 0;
  selectedOption: QuestionOption = {} as QuestionOption;
  displayResultPanel = false;
  lessonEnded = false;

  get currentQuestion() {
    return this.lesson && this.lesson.questions[this.currentQuestionIndex];
  }

  get currentScore() {
    return this.lesson.questions.map(q => (q.answeredCorrectly ? 1 : 0) as number).reduce((a, b) => a + b);
  }

  get isAnswerCorrect() {
    return this.correctAnswer.id === this.selectedOption.id && this.correctAnswer.typeId === this.selectedOption.typeId;
  }

  get correctAnswer() {
    return this.currentQuestion.options.filter(x => x.isCorrectAnswer)[0];
  }

  get imageOptions() {
    return this.currentQuestion.options.filter(o => o.typeId === 2);
  }

  get textOptions() {
    return this.currentQuestion.options.filter(o => o.typeId === 1);
  }

  constructor(private route: ActivatedRoute,
    private router: Router,
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

  setSelectedOption(option: QuestionOption) {
    this.selectedOption = option;
  }

  moveToNext() {
    this.selectedOption = {} as QuestionOption;
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
    this.lessonEnded = true;
    this.displayResultPanel = true;
    console.log('finish lesson', this.lesson.questions);
  }

  continue() {
    if (this.lessonEnded) {
      this.router.navigate(['lessons']);

    } else {
      this.moveToNext();
    }
  }
}
