﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OrdersService.API.Data;

#nullable disable

namespace OrdersService.API.Data.Migrations
{
    [DbContext(typeof(OrderDbContext))]
    [Migration("20250320113056_UpdateStepTypeForSagaContextTables")]
    partial class UpdateStepTypeForSagaContextTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("OrderService")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OrdersService.API.Data.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RejectComment")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Orders", "OrderService");
                });

            modelBuilder.Entity("OrdersService.API.Data.Entities.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems", "OrderService");
                });

            modelBuilder.Entity("OrdersService.API.Data.Entities.OrderSagaContext", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<int>("Step")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("OrderId");

                    b.ToTable("OrderSagaContexts", "OrderService");
                });

            modelBuilder.Entity("OrdersService.API.Data.Entities.Product", b =>
                {
                    b.Property<Guid>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ProductId");

                    b.ToTable("Products", "OrderService");
                });

            modelBuilder.Entity("OrdersService.API.Data.Entities.OrderItem", b =>
                {
                    b.HasOne("OrdersService.API.Data.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OrdersService.API.Data.Entities.OrderSagaContext", b =>
                {
                    b.OwnsMany("OrdersService.API.Data.Entities.SagaOrderItem", "OrderItems", b1 =>
                        {
                            b1.Property<Guid>("OrderSagaContextOrderId")
                                .HasColumnType("uuid");

                            b1.Property<int>("__synthesizedOrdinal")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            b1.Property<Guid>("ProductId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Quantity")
                                .HasColumnType("integer");

                            b1.HasKey("OrderSagaContextOrderId", "__synthesizedOrdinal");

                            b1.ToTable("OrderSagaContexts", "OrderService");

                            b1.ToJson("OrderItems");

                            b1.WithOwner()
                                .HasForeignKey("OrderSagaContextOrderId");
                        });

                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("OrdersService.API.Data.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
