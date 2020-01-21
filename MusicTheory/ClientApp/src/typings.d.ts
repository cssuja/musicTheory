interface QuestionModel {
    id: number;
    questionText: string;
    answerId: number;
    options: QuestionOption[];
    answeredCorrectly: boolean;
    typeId: number;
}

interface QuestionOption {
    id: number;
    option: any;
}

interface Lesson {
    id: number;
    name: string;
    questions: QuestionModel[]
}
