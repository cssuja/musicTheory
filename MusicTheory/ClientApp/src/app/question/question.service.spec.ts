import { TestBed } from '@angular/core/testing';

import { QuestionService } from './question.service';
import {
    HttpClientTestingModule,
    HttpTestingController
} from '@angular/common/http/testing';
import { log } from 'util';

describe('QuestionService', () => {
    let httpTestingController: HttpTestingController;
    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule]
        });
        httpTestingController = TestBed.get(HttpTestingController);
    });

    afterEach(() => {
        httpTestingController.verify();
    });

    it('should be created', () => {
        const service: QuestionService = TestBed.get(QuestionService);
        expect(service).toBeTruthy();
    });

    describe('getLesson', () => {
        it('should call the correct endpoint and return the lesson', () => {
            const service: QuestionService = TestBed.get(QuestionService);
            const id = 3;
            const expectedLesson = <Lesson>{
                id: id,
                name: 'A Name',
                questions: [
                    <Question>{

                    }
                ]
            };

            service.getLesson(id).subscribe(response => {
                expect(response).toBe(expectedLesson);
            });

            const req = httpTestingController.expectOne('https://localhost:44366/api/question?id=3');

            expect(req.request.method).toEqual('GET');
            req.flush(expectedLesson);
        });
    });
});
