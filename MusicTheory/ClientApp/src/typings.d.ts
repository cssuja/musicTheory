interface Question {
    id: number;
    text: string;
    options: QuestionOption[];
    answeredCorrectly: boolean;
}

interface QuestionOption {
    id: number;
    option: any;
    isCorrectAnswer: boolean;
    typeId: number;
}

interface Lesson {
    id: number;
    name: string;
    questions: Question[]
}

interface SelectItem {
    id: number;
    display: object;
    typeId: number;
}
