using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAPI.Migrations
{
    /// <inheritdoc />
    public partial class with_foreign_keys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_patients_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Doctor_DoctorId",
                table: "DoctorSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpeciality_Speciality_SpecialityId",
                table: "DoctorSpeciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Speciality",
                table: "Speciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSpeciality",
                table: "DoctorSpeciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Speciality",
                newName: "specialities");

            migrationBuilder.RenameTable(
                name: "DoctorSpeciality",
                newName: "doctorSpecialities");

            migrationBuilder.RenameTable(
                name: "Doctor",
                newName: "doctors");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "appointments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "doctorSpecialities",
                newName: "SerialNumber");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpeciality_SpecialityId",
                table: "doctorSpecialities",
                newName: "IX_doctorSpecialities_SpecialityId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpeciality_DoctorId",
                table: "doctorSpecialities",
                newName: "IX_doctorSpecialities_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_PatientId",
                table: "appointments",
                newName: "IX_appointments_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_DoctorId",
                table: "appointments",
                newName: "IX_appointments_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_specialities",
                table: "specialities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_doctorSpecialities",
                table: "doctorSpecialities",
                column: "SerialNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_doctors",
                table: "doctors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appointments",
                table: "appointments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_doctors_DoctorId",
                table: "appointments",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_patients_PatientId",
                table: "appointments",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctorSpecialities_doctors_DoctorId",
                table: "doctorSpecialities",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_doctorSpecialities_specialities_SpecialityId",
                table: "doctorSpecialities",
                column: "SpecialityId",
                principalTable: "specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_doctors_DoctorId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_appointments_patients_PatientId",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_doctorSpecialities_doctors_DoctorId",
                table: "doctorSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_doctorSpecialities_specialities_SpecialityId",
                table: "doctorSpecialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_specialities",
                table: "specialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_doctorSpecialities",
                table: "doctorSpecialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_doctors",
                table: "doctors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appointments",
                table: "appointments");

            migrationBuilder.RenameTable(
                name: "specialities",
                newName: "Speciality");

            migrationBuilder.RenameTable(
                name: "doctorSpecialities",
                newName: "DoctorSpeciality");

            migrationBuilder.RenameTable(
                name: "doctors",
                newName: "Doctor");

            migrationBuilder.RenameTable(
                name: "appointments",
                newName: "Appointment");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "DoctorSpeciality",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_doctorSpecialities_SpecialityId",
                table: "DoctorSpeciality",
                newName: "IX_DoctorSpeciality_SpecialityId");

            migrationBuilder.RenameIndex(
                name: "IX_doctorSpecialities_DoctorId",
                table: "DoctorSpeciality",
                newName: "IX_DoctorSpeciality_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_PatientId",
                table: "Appointment",
                newName: "IX_Appointment_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_appointments_DoctorId",
                table: "Appointment",
                newName: "IX_Appointment_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Speciality",
                table: "Speciality",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSpeciality",
                table: "DoctorSpeciality",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doctor",
                table: "Doctor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                table: "Appointment",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_patients_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Doctor_DoctorId",
                table: "DoctorSpeciality",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpeciality_Speciality_SpecialityId",
                table: "DoctorSpeciality",
                column: "SpecialityId",
                principalTable: "Speciality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
