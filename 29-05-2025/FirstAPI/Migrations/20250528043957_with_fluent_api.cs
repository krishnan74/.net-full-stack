using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstAPI.Migrations
{
    /// <inheritdoc />
    public partial class with_fluent_api : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "PK_appointments",
                table: "appointments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "appointments",
                newName: "AppointmentNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppointmentNumber",
                table: "appointments",
                column: "AppointmentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Appoinment_Doctor",
                table: "appointments",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appoinment_Patient",
                table: "appointments",
                column: "PatientId",
                principalTable: "patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Speciality_Doctor",
                table: "doctorSpecialities",
                column: "DoctorId",
                principalTable: "doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Speciality_Spec",
                table: "doctorSpecialities",
                column: "SpecialityId",
                principalTable: "specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appoinment_Doctor",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appoinment_Patient",
                table: "appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Speciality_Doctor",
                table: "doctorSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_Speciality_Spec",
                table: "doctorSpecialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppointmentNumber",
                table: "appointments");

            migrationBuilder.RenameColumn(
                name: "AppointmentNumber",
                table: "appointments",
                newName: "Id");

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
    }
}
