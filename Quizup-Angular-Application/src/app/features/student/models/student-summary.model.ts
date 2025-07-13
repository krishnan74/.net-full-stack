export interface StudentSummary {
  studentId: number;
  studentName: string;
  studentEmail: string;
  studentClass: string;
  totalQuizzesAvailable: number;
  totalQuizzesStarted: number;
  totalQuizzesCompleted: number;
  totalQuizzesInProgress: number;
  totalQuizzesSaved: number;
  averageScore: number;
  highestScore: number;
  lowestScore: number;
  totalQuestionsAttempted: number;
  totalCorrectAnswers: number;
  accuracyPercentage: number;
  totalTimeSpentMinutes: number;
  quizzesByStatus: any;
  recentActivity: any;
  performanceTrend: any;
}