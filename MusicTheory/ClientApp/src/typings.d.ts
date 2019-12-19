interface QuestionModel {
    id: number;
    questionText: string;
    answerId: number;
    textOptions: TextQuestionOption[];
    imageOptions: ImageQuestionOption[];
}

interface IQuestionOption {
    id: number;
    questionId: number;
}

interface TextQuestionOption extends IQuestionOption {
    text: string;
}

interface ImageQuestionOption extends IQuestionOption {
    image: Blob;
}

interface Lesson {
    id: number;
    name: string;
    questions: QuestionModel[]
}
