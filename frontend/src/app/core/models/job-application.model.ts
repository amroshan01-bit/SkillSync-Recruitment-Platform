export interface JobApplication {
  id: string;
  vacancyId: string;
  jobSeekerUserId: string;
  jobTitle: string;
  companyName: string;
  status: string;
  coverLetter: string | null;
  appliedAtUtc: string;
  updatedAtUtc: string;
}

export interface CreateJobApplicationRequest {
  vacancyId: string;
  coverLetter: string;
}

export interface UpdateApplicationStatusRequest {
  status: string;
}

export interface RankedApplicant {
  applicationId: string;
  vacancyId: string;
  jobSeekerUserId: string;
  fullName: string;
  professionalTitle: string | null;
  location: string | null;
  yearsOfExperience: number;
  highestQualification: string | null;
  status: string;
  coverLetter: string | null;
  matchScore: number;
  matchedSkills: string[];
  missingSkills: string[];
  appliedAtUtc: string;
}