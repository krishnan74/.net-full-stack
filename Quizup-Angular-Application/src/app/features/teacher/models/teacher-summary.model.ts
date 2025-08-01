export interface TeacherSummary {
  teacherId: number;
  teacherName: string;
  teacherEmail: string;
  teacherSubjects: any;
  totalQuizzesCreated: number;
  totalActiveQuizzes: number;
  totalInactiveQuizzes: number;
  totalQuestionsCreated: number;
  totalStudentSubmissions: number;
  totalStudentsParticipated: number;
  averageCompletionRate: number;
  averageStudentScore: number;
  highestQuizScore: number;
  lowestQuizScore: number;
  totalQuestionsAnswered: number;
  totalCorrectAnswers: number;
  overallAccuracyPercentage: number;
  quizzesByStatus: any;
  studentPerformanceSummary: any;
  recentQuizActivity: any;
  quizPerformanceTrend: any;
  recentQuizStats?: {
    quizTitle: string;
    avgScore: number;
    completionRate: number;
    totalAttempts: number;
    lastAttempted: string;
  }[];
}
