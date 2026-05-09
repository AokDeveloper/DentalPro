export interface Appointment {
    id: string;
    patientId: string;
    patientName: string; // Listede göstermek için (Backend'den gelmeli veya join yapılmalı)
    date: string; // Backend genelde string tarih döner
    notes?: string;    // Yapılacak işlem notu
    status?: number;         // 'Onaylandı', 'Tamamlandı' vb. 
   
}




 
    
  
