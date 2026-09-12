export interface MatchResult {
  vacancyId: string;
  jobTitle: string;
  companyName: string;
  location: string;
  workplaceType: string;
  employmentType: string;
  minimumSalary: number;
  maximumSalary: number;
  currency: string;
  matchScore: number;
  matchedSkills: string[];
  missingSkills: string[];
  hasApplied: boolean;
  applicationStatus: string | null;
}

export interface UpdateJobSeekerSkillsRequest {
  skills: string[];
}