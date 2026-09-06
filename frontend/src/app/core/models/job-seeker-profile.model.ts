export interface JobSeekerProfile {
  id: string;
  userId: string;
  fullName: string;
  professionalTitle: string | null;
  bio: string | null;
  location: string | null;
  phoneNumber: string | null;
  yearsOfExperience: number;
  highestQualification: string | null;
  createdAtUtc: string;
  updatedAtUtc: string;
}

export interface CreateJobSeekerProfile {
  userId: string;
  fullName: string;
  professionalTitle: string | null;
  bio: string | null;
  location: string | null;
  phoneNumber: string | null;
  yearsOfExperience: number;
  highestQualification: string | null;
}

export interface UpdateJobSeekerProfile {
  fullName: string;
  professionalTitle: string | null;
  bio: string | null;
  location: string | null;
  phoneNumber: string | null;
  yearsOfExperience: number;
  highestQualification: string | null;
}