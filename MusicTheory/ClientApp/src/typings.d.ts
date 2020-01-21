interface QuestionModel {
    id: number;
    questionText: string;
    options: QuestionOption[];
    answeredCorrectly: boolean;
    typeId: number;
}

interface QuestionOption {
    id: number;
    option: any;
    isCorrectAnswer: boolean;

}

interface Lesson {
    id: number;
    name: string;
    questions: QuestionModel[]
}
