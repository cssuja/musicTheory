import { TestBed } from '@angular/core/testing';

import { QuestionService } from './question.service';

describe('QuestionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: QuestionService = TestBed.get(QuestionService);
    expect(service).toBeTruthy();
  });

    describe('getQuestion', () => {
        it('should call the correct endpoint and return the question', () => {
            const service: QuestionService = TestBed.get(QuestionService);
            const id = 3;
          //  const actual= service.getQuestion(id);
        });
    });
});
