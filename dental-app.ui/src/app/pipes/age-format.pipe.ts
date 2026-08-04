import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ageFormat',
  standalone: true
})
export class AgeFormatPipe implements PipeTransform {
  transform(value: string | Date): string {
    if (!value) return '-';

    const bDate = new Date(value);
    const today = new Date();

    let years = today.getFullYear() - bDate.getFullYear();
    let months = today.getMonth() - bDate.getMonth();

    if (months < 0 || (months === 0 && today.getDate() < bDate.getDate())) {
      years--;
      months += (months < 0 ? 12 : 11);
    }

    if (years === 0 && months === 0) return 'Yeni Doğan';
    if (years === 0) return `${months} aylık`;
    if (months === 0) return `${years} yaşında`;
    
    return `${years} yaş, ${months} ay`;
  }
}