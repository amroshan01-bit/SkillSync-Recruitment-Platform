import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatchingService } from '../../../core/services/matching.service';

@Component({
  selector: 'app-manage-skills',
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './manage-skills.component.html',
  styleUrl: './manage-skills.component.css',
})
export class ManageSkillsComponent implements OnInit {
  private readonly userId =
    '11111111-1111-1111-1111-111111111111';

  skills: string[] = [];
  newSkill = '';

  isLoading = false;
  isSaving = false;

  successMessage = '';
  errorMessage = '';

  constructor(
    private readonly matchingService: MatchingService
  ) {}

  ngOnInit(): void {
    this.loadSkills();
  }

  loadSkills(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.matchingService.getSkills(this.userId).subscribe({
      next: (skills: string[]) => {
        this.skills = skills;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load your skills.';
        this.isLoading = false;
      },
    });
  }

  addSkill(): void {
    const skill = this.newSkill.trim();

    if (!skill) {
      return;
    }

    const alreadyExists = this.skills.some(
      (currentSkill) =>
        currentSkill.toLowerCase() === skill.toLowerCase()
    );

    if (alreadyExists) {
      this.errorMessage = 'This skill is already added.';
      return;
    }

    this.skills.push(skill);
    this.newSkill = '';
    this.errorMessage = '';
    this.successMessage = '';
  }

  removeSkill(skill: string): void {
    this.skills = this.skills.filter(
      (currentSkill) => currentSkill !== skill
    );

    this.successMessage = '';
  }

  saveSkills(): void {
    if (this.skills.length === 0) {
      this.errorMessage = 'Add at least one skill.';
      return;
    }

    this.isSaving = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.matchingService
      .updateSkills({
        userId: this.userId,
        skills: this.skills,
      })
      .subscribe({
        next: () => {
          this.successMessage =
            'Your skills were updated successfully.';
          this.isSaving = false;
        },
        error: () => {
          this.errorMessage =
            'Unable to update your skills.';
          this.isSaving = false;
        },
      });
  }
}