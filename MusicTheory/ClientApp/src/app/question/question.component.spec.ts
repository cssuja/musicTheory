import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionComponent } from './question.component';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs';
import { QuestionService } from './question.service';

describe('QuestionComponent', () => {
  let component: QuestionComponent;
  let fixture: ComponentFixture<QuestionComponent>;
  const mockActivatedRoute = jasmine.createSpyObj<ActivatedRoute>(['params']);
  const mockQuestionService = jasmine.createSpyObj<QuestionService>(['getLesson']);
  const expectedLessonId = 3;
  mockActivatedRoute.params = of({ lessonId: expectedLessonId });
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [QuestionComponent],
      providers: [
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: QuestionService, useValue: mockQuestionService }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionComponent);
    component = fixture.componentInstance;

  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });
  describe('onInit', () => {
    it('should call the service to get a lesson and display the first question', async(() => {
      const expectedLesson = <Lesson>{
        id: expectedLessonId,
        name: 'test',
        questions: <QuestionModel[]>[
          {
            text: 'Which is the right answer?',
            options: <QuestionOption[]>[
              {
                id: 1,
                option: 'some text 1',
              },
              {
                id: 2,
                option: 'some text 2',
              }
            ]
          }
        ]
      };
      mockQuestionService.getLesson.and.returnValue(of(expectedLesson));
      fixture.detectChanges();
      fixture.whenStable().then(() => {
        expect(mockQuestionService.getLesson).toHaveBeenCalledWith(expectedLessonId);
        const text = fixture.nativeElement.textContent;
        expect(text).toContain(expectedLesson.questions[0].text);
        expect(text).toContain(expectedLesson.questions[0].options[0].option);
        expect(text).toContain(expectedLesson.questions[0].options[1].option);
      });
    }));
  });
});
