﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

#nullable disable

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VyazmaTech.Prachka.Domain.Kernel.Entity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Id");

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("VyazmaTech.Prachka.Infrastructure.DataAccess.Models.OutboxMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<string>("Error")
                        .HasColumnType("text")
                        .HasColumnName("error");

                    b.Property<DateTime>("OccuredOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("occured_on_utc");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("processed_on_utc");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_outbox_messages");

                    b.HasIndex("ProcessedOnUtc")
                        .HasDatabaseName("ix_outbox_messages_processed_on_utc")
                        .HasFilter("processed_on_utc is null");

                    b.ToTable("outbox_messages");
                });

            modelBuilder.Entity("VyazmaTech.Prachka.Infrastructure.DataAccess.Models.QueueJobMessage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("JobId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("job_id");

                    b.Property<Guid>("QueueId")
                        .HasColumnType("uuid")
                        .HasColumnName("queue_id");

                    b.HasKey("Id")
                        .HasName("pk_queue_job_messages");

                    b.HasIndex("QueueId", "JobId")
                        .IsUnique()
                        .HasDatabaseName("ix_queue_job_messages_queue_id_job_id")
                        .HasAnnotation("Npgsql:CreatedConcurrently", true);

                    b.ToTable("queue_job_messages");
                });

            modelBuilder.Entity("VyazmaTech.Prachka.Domain.Core.Orders.Order", b =>
                {
                    b.HasBaseType("VyazmaTech.Prachka.Domain.Kernel.Entity");

                    b.Property<DateOnly>("CreationDate")
                        .HasColumnType("date")
                        .HasColumnName("creation_date");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date_time");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("status");

                    b.Property<Guid>("queue_id")
                        .HasColumnType("uuid")
                        .HasColumnName("queue_id");

                    b.Property<Guid>("user_id")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasIndex("queue_id", "user_id")
                        .HasDatabaseName("ix_orders_queue_id_user_id");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("VyazmaTech.Prachka.Domain.Core.Queues.Queue", b =>
                {
                    b.HasBaseType("VyazmaTech.Prachka.Domain.Kernel.Entity");

                    b.Property<DateOnly>("CreationDate")
                        .HasColumnType("date")
                        .HasColumnName("creation_date");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<int>("State")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("state");

                    b.Property<DateOnly>("assignment_date")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("date")
                        .HasColumnName("assignment_date");

                    b.ComplexProperty<Dictionary<string, object>>("ActivityBoundaries", "VyazmaTech.Prachka.Domain.Core.Queues.Queue.ActivityBoundaries#QueueActivityBoundaries", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<TimeOnly>("ActiveFrom")
                                .HasColumnType("time without time zone")
                                .HasColumnName("active_from");

                            b1.Property<TimeOnly>("ActiveUntil")
                                .HasColumnType("time without time zone")
                                .HasColumnName("active_until");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("AssignmentDate", "VyazmaTech.Prachka.Domain.Core.Queues.Queue.AssignmentDate#AssignmentDate", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateOnly>("Value")
                                .ValueGeneratedOnUpdateSometimes()
                                .HasColumnType("date")
                                .HasColumnName("assignment_date");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Capacity", "VyazmaTech.Prachka.Domain.Core.Queues.Queue.Capacity#Capacity", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("capacity");
                        });

                    b.HasIndex("assignment_date")
                        .IsUnique()
                        .IsDescending()
                        .HasDatabaseName("ix_queues_assignment_date");

                    b.ToTable("queues");
                });

            modelBuilder.Entity("VyazmaTech.Prachka.Domain.Core.Users.User", b =>
                {
                    b.HasBaseType("VyazmaTech.Prachka.Domain.Kernel.Entity");

                    b.Property<DateOnly>("CreationDate")
                        .HasColumnType("date")
                        .HasColumnName("creation_date");

                    b.Property<DateTime?>("ModifiedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified_on_utc");

                    b.Property<string>("fullname")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("text")
                        .HasColumnName("fullname");

                    b.Property<string>("telegram_username")
                        .ValueGeneratedOnUpdateSometimes()
                        .HasColumnType("text")
                        .HasColumnName("telegram_username");

                    b.ComplexProperty<Dictionary<string, object>>("Fullname", "VyazmaTech.Prachka.Domain.Core.Users.User.Fullname#Fullname", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .ValueGeneratedOnUpdateSometimes()
                                .HasColumnType("text")
                                .HasColumnName("fullname");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("TelegramUsername", "VyazmaTech.Prachka.Domain.Core.Users.User.TelegramUsername#TelegramUsername", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .ValueGeneratedOnUpdateSometimes()
                                .HasColumnType("text")
                                .HasColumnName("telegram_username");
                        });

                    b.HasIndex("fullname", "telegram_username")
                        .HasDatabaseName("ix_users_fullname_telegram_username");

                    b.ToTable("users");
                });

            modelBuilder.Entity("VyazmaTech.Prachka.Domain.Core.Orders.Order", b =>
                {
                    b.HasOne("VyazmaTech.Prachka.Domain.Core.Queues.Queue", "Queue")
                        .WithMany("Orders")
                        .HasForeignKey("queue_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_queues_queue_id");

                    b.HasOne("VyazmaTech.Prachka.Domain.Core.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_users_user_id");

                    b.Navigation("Queue");

                    b.Navigation("User");
                });

            modelBuilder.Entity("VyazmaTech.Prachka.Domain.Core.Queues.Queue", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
